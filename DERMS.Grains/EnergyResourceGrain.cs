using System.Timers;
using DERMS.Dto;
using DERMS.Interfaces;

namespace DERMS.Grains
{
    public class EnergyResourceGrain : Grain, IEnergyResourceGrain
    {
        private bool _isActive;
        private bool _isConnectedToGrid;
        private double _energyOutput;
        private double _energyGeneration = 0;
        private System.Timers.Timer? _energyFluctuationTimer;
        private string _owner = String.Empty;
        private string _name = String.Empty;

        public Task Activate()
        {
            _isActive = true;
            _energyFluctuationTimer = new System.Timers.Timer(5000); // 5-second interval
            _energyFluctuationTimer.Elapsed += OnEnergyFluctuation;
            _energyFluctuationTimer.Start();
            return Task.CompletedTask;
        }
        
        public Task Deactivate()
        {
            _isActive = false;
            _energyFluctuationTimer?.Stop();
            _energyFluctuationTimer?.Dispose();
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

        private void OnEnergyFluctuation(object? sender, ElapsedEventArgs e)
        {
            _energyGeneration = Math.Round((_energyGeneration + 0.1) % 5, 2);
        }
    }
}