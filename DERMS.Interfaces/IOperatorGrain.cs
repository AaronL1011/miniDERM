using Orleans;
using DERMS.Dto;

namespace DERMS.Interfaces;
public interface IOperatorGrain : IGrainWithStringKey
{
    Task<Guid> AddEnergyResource();
    Task RemoveEnergyResource(Guid resourceId);
    Task<IEnergyResourceGrain?> GetEnergyResource(Guid resourceId);
    Task<int> GetEnergyResourceCount();
    Task<List<ResourceInfo>> GetEnergyResourceInfo();
}
