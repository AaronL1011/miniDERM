using Orleans;
using DERMS.Interfaces;
using DERMS.Dto;

namespace DERMS.Grains;
public class OperatorGrain : Grain, IOperatorGrain
    {
        private readonly Dictionary<Guid, IEnergyResourceGrain> _resources = new();

        public Task<Guid> AddEnergyResource()
        {
            var resourceId = Guid.NewGuid();
            var resource = GrainFactory.GetGrain<IEnergyResourceGrain>(resourceId);
            _resources.Add(resourceId, resource);
            return Task.FromResult(resourceId);
        }

        public async Task RemoveEnergyResource(Guid resourceId)
        {
            var resource = GrainFactory.GetGrain<IEnergyResourceGrain>(resourceId);
            await resource.DisconnectFromGrid();
            _resources.Remove(resourceId);
        }

        public Task<IEnergyResourceGrain?> GetEnergyResource(Guid resourceId)
        {
            _resources.TryGetValue(resourceId, out var resource);
            return Task.FromResult(resource);
        }

        public Task<int> GetEnergyResourceCount()
        {
            return Task.FromResult(_resources.Count);
        }

        public async Task<ResourceInfo[]> GetEnergyResourceInfo()
        {
            var resourceInfos = _resources.Select(async r =>
            {
                var resource = await r.Value.ToDTO();
                resource.Id = r.Key.ToString();
                return resource;
            }).ToList();

            var resourceList = await Task.WhenAll(resourceInfos);

            return resourceList;
        }
    }
