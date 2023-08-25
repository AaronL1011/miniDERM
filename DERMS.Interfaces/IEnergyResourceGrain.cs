using DERMS.Dto;

namespace DERMS.Interfaces
{
    public interface IEnergyResourceGrain : IGrainWithGuidKey
    {
        Task Activate();
        Task Deactivate();
        Task<string> GetStatus();
        Task SetEnergyOutput(double output);
        Task<double> GetEnergyOutput();
        Task<double> GetEnergyGeneration();
        Task<EnergyTimestamp[]> GetEnergyGenerationHistory();
        Task ConnectToGrid();
        Task DisconnectFromGrid();
        Task<bool> IsConnectedToGrid();
        Task SetOwner(string operatorName);
        Task<string> GetOwner();
        Task SetName(string name);
        Task<string> GetName();
        Task<ResourceInfo> ToDTO();
    }
}