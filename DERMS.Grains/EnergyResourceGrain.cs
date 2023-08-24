using Orleans;
using DERMS.Interfaces;

namespace DERMS.Grains
{
    public class EnergyResourceGrain : Grain, IEnergyResourceGrain
    {
        private bool _isActive;
        private bool _isConnectedToGrid;
        private double _energyOutput;
        private string _owner = String.Empty;
        private string _name = String.Empty;

        public Task Activate()
        {
            _isActive = true;
            return Task.CompletedTask;
        }
        
        public Task Deactivate()
        {
            _isActive = false;
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

        public Task SetOwner(string username)
        {
            _owner = username;
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
    }
}