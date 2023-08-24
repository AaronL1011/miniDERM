using Orleans;

namespace DERMS.Interfaces
{
    public interface IEnergyResourceGrain : IGrainWithGuidKey
    {
        Task Activate();
        Task Deactivate();
        Task<string> GetStatus();
        Task SetEnergyOutput(double output);
        Task<double> GetEnergyOutput();
        Task ConnectToGrid();
        Task DisconnectFromGrid();
        Task<bool> IsConnectedToGrid();
        Task SetOwner(string username);
        Task<string> GetOwner();
        Task SetName(string name);
        Task<string> GetName();
    }
}