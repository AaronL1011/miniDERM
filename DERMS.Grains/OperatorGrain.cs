﻿using Orleans;
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

    public async Task<EnergyTimestamp[]> GetEnergyHistory()
    {
        return await GetHistory(async (r) => await r.GetEnergyGenerationHistory());
    }

    public async Task<EnergyTimestamp[]> GetOutputHistory()
    {
        return await GetHistory(async (r) => await r.GetEnergyOutputHistory());
    }

    public delegate Task<EnergyTimestamp[]> GetHistoryDelegate(IEnergyResourceGrain resource);

    public async Task<EnergyTimestamp[]> GetHistory(GetHistoryDelegate getHistoryMethod)
    {
        var result = new Dictionary<DateTime, Dictionary<string, Tuple<double, int>>>();  // Outer: Time, Inner: Resource, Tuple: Sum, Count

        foreach (var r in _resources)
        {
            var resourceName = r.Key.ToString();  // Assuming resource name is the key
            var historyData = await getHistoryMethod(r.Value);

            foreach (var history in historyData)
            {
                if (result.TryGetValue(history.Time, out var innerDict))
                {
                    if (innerDict.TryGetValue(resourceName, out var value))
                    {
                        double newSum = Math.Round(value.Item1 + history.Amount, 2);
                        int newCount = value.Item2 + 1;
                        innerDict[resourceName] = Tuple.Create(newSum, newCount);
                    }
                    else
                    {
                        innerDict.Add(resourceName, Tuple.Create(history.Amount, 1));
                    }
                }
                else
                {
                    var newInnerDict = new Dictionary<string, Tuple<double, int>>
                {
                    { resourceName, Tuple.Create(history.Amount, 1) }
                };

                    result.Add(history.Time, newInnerDict);
                }
            }
        }

        return result.Select(r =>
            new EnergyTimestamp()
            {
                Time = r.Key,
                Amount = Math.Round(r.Value.Values.Sum(inner => inner.Item1 / inner.Item2), 2)
            }).ToArray();
    }
}
