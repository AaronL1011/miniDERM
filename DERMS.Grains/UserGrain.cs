using Orleans;
using DERMS.Interfaces;
using DERMS.Dto;

namespace DERMS.Grains;
public class UserGrain : Grain, IUserGrain
    {
        private readonly Dictionary<Guid, IEnergyResourceGrain> _resources = new();

        public Task<Guid> AddEnergyResource()
        {
            var resourceId = Guid.NewGuid();
            var resource = GrainFactory.GetGrain<IEnergyResourceGrain>(resourceId);
            _resources.Add(resourceId, resource);
            return Task.FromResult(resourceId);
        }

        public Task RemoveEnergyResource(Guid resourceId)
        {
            _resources.Remove(resourceId);
            return Task.CompletedTask;
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

        public Task<List<ResourceInfo>> GetEnergyResourceInfo()
        {
            var resourceList = _resources.Select(r =>
            {
                var resource = r.Value;
                return new ResourceInfo()
                {
                    Id = r.Key.ToString(),
                    Name = resource.GetName().GetAwaiter().GetResult(),
                    EnergyOutput = resource.GetEnergyOutput().GetAwaiter().GetResult(),
                    Status = resource.GetStatus().GetAwaiter().GetResult(),
                    IsConnectedToGrid = resource.IsConnectedToGrid().GetAwaiter().GetResult(),
                    Owner = resource.GetOwner().GetAwaiter().GetResult(),
                };
            }).ToList();

            if (resourceList == null) {
                var emptyList = new List<ResourceInfo>();
                return Task.FromResult(emptyList);
            }

            return Task.FromResult(resourceList);
        }
    }
