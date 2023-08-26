using DERMS.Dto;

namespace DERMS.Silo
{
    class EnergyResourceInfo {
        public ResourceInfo[]? Resources { set; get; }
        public EnergyTimestamp[]? EnergyHistory { set; get; }
        public EnergyTimestamp[]? OutputHistory { set; get; }
        public double TotalGeneration { set; get; }
        public double CurrentOutput { set; get; }
    }
}