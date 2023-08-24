using Orleans;

namespace DERMS.Dto
{
    [GenerateSerializer]
    public class ResourceInfo
    {
        [Id(0)]
        public string Id { get; set; } = String.Empty;
        [Id(1)]
        public string Name { get; set; } = String.Empty;
        [Id(2)]
        public string Status { get; set; } = "Inactive";
        [Id(3)]
        public double EnergyOutput { get; set; } = 0;
        [Id(4)]
        public bool IsConnectedToGrid { get; set; }
        [Id(5)]
        public string Owner { get; set; } = String.Empty;
    }
}