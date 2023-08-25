using Orleans;

namespace DERMS.Dto
{
    [GenerateSerializer]
    public class EnergyTimestamp
    {
        [Id(0)]
        public DateTime Time { get; set; }
        [Id(1)]
        public double Amount { get; set; }
    }
}