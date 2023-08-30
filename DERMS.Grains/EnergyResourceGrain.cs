using System.Timers;
using DERMS.Dto;
using DERMS.Interfaces;
using NodaTime;
using NodaTime.TimeZones;

namespace DERMS.Grains
{
    public class EnergyResourceGrain : Grain, IEnergyResourceGrain
    {
        const int MaxHistorySize = 720; // Logging every 5 seconds, 12 logs per minute, 60 minutes worth of logs = 720 EnergyTimestamp objects == ~30KB max size;
        private Random _random = new Random();
        private bool _isActive;
        private bool _isConnectedToGrid;
        private double _energyOutput;
        private double _energyGeneration = 0;
        private System.Timers.Timer? _energyFluctuationTimer;
        private string _owner = String.Empty;
        private string _name = String.Empty;
        private NodaTime.DateTimeZone? _timeZone;
        private readonly Queue<EnergyTimestamp> _generationHistory = new Queue<EnergyTimestamp>(MaxHistorySize);
        private readonly Queue<EnergyTimestamp> _outputHistory = new Queue<EnergyTimestamp>(MaxHistorySize);

        public Task Activate()
        {
            _isActive = true;
            _energyFluctuationTimer = new System.Timers.Timer(5000); // 5-second interval
            _energyFluctuationTimer.Elapsed += OnEnergyTick;
            _energyFluctuationTimer.Start();
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            _isActive = false;
            _energyFluctuationTimer?.Stop();
            _energyFluctuationTimer?.Dispose();
            _energyFluctuationTimer = null;
            _energyGeneration = 0;
            return Task.CompletedTask;
        }

        public Task<string> GetStatus()
        {
            return Task.FromResult(_isActive ? "Active" : "Inactive");
        }

        public Task SetEnergyOutput(double output)
        {
            _energyOutput = Math.Clamp(output, 0, 100);
            return Task.CompletedTask;
        }

        public Task<double> GetEnergyOutput()
        {
            return Task.FromResult(_energyOutput);
        }

        public Task<double> GetEnergyGeneration()
        {
            return Task.FromResult(_energyGeneration);
        }

        public Task ConnectToGrid()
        {
            _isConnectedToGrid = true;
            return Task.CompletedTask;
        }

        public Task DisconnectFromGrid()
        {
            _isConnectedToGrid = false;
            return Task.CompletedTask;
        }

        public Task<bool> IsConnectedToGrid()
        {
            return Task.FromResult(_isConnectedToGrid);
        }

        public Task SetOwner(string operatorName)
        {
            _owner = operatorName;
            return Task.CompletedTask;
        }

        public Task<string> GetOwner()
        {
            return Task.FromResult(_owner);
        }

        public Task SetName(string name)
        {
            _name = name;
            return Task.CompletedTask;
        }

        public Task<string> GetName()
        {
            return Task.FromResult(_name);
        }

        public Task SetTimeZone(string timeZone)
        {
            var tzdbSource = TzdbDateTimeZoneSource.Default;
            if (!tzdbSource.CanonicalIdMap.ContainsKey(timeZone))
            {
                throw new ArgumentException("Invalid IANA time zone ID");
            }
            var nodaTimeZone = tzdbSource.ForId(timeZone);
            _timeZone = nodaTimeZone;
            return Task.CompletedTask;
        }

        public Task<ResourceInfo> ToDTO()
        {
            return Task.FromResult(new ResourceInfo
            {
                Name = _name,
                Status = _isActive ? "Active" : "Inactive",
                IsConnectedToGrid = _isConnectedToGrid,
                EnergyOutput = _energyOutput,
                EnergyGeneration = _energyGeneration,
                Owner = _owner,
            });
        }

        public Task<EnergyTimestamp[]> GetEnergyGenerationHistory()
        {
            var history = _generationHistory.ToArray();
            return Task.FromResult(history);
        }

        public Task<EnergyTimestamp[]> GetEnergyOutputHistory()
        {
            var history = _outputHistory.ToArray();
            return Task.FromResult(history);
        }

        private void OnEnergyTick(object? sender, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;

            SimulateEnergyGenerationForLocalTime(now);

            DateTime currentTimeToTheMinute = new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerMinute), now.Kind);
            var energyOutput = _isConnectedToGrid ? (_energyOutput / 100) * _energyGeneration : 0;
            
            LogEnergyHistory(currentTimeToTheMinute, _energyGeneration, energyOutput);
        }

        private void SimulateEnergyGenerationForLocalTime(DateTime now)
        {
            if (_timeZone == null) return;

            var instant = Instant.FromDateTimeUtc(now.ToUniversalTime());
            var zonedDateTime = instant.InZone(_timeZone);
            double timeOfDay = zonedDateTime.Hour;

            if (timeOfDay < 6 || timeOfDay >= 18) // no solar generation at night time
            {
                _energyGeneration = 0;
                return;
            }

            double peakHour = 12.0;
            double standardDeviation = 2.5;
            double peakEnergy = 5.0;
            double fluctuation = _random.NextDouble() - 0.5;

            double gaussianCurveValue = Math.Pow(peakEnergy * Math.Exp(-0.5 * Math.Pow((timeOfDay - peakHour) / standardDeviation, 2)), 0.5);


            _energyGeneration = Math.Clamp(Math.Round(gaussianCurveValue + fluctuation, 2), 0, 5);
        }

        private void LogEnergyHistory(DateTime currentMinute, double generation, double output)
        {
            if (_generationHistory.Count >= MaxHistorySize)
            {
                _generationHistory.Dequeue();
            }
            if (_outputHistory.Count >= MaxHistorySize)
            {
                _outputHistory.Dequeue();
            }

            _generationHistory.Enqueue(new EnergyTimestamp { Time = currentMinute, Amount = _energyGeneration });
            _outputHistory.Enqueue(new EnergyTimestamp { Time = currentMinute, Amount = output });
        }
    }
}