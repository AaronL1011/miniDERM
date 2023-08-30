namespace DERMS.Silo
{
    public class CreateEnergyResource
    {
        public string Name {get; set;} = String.Empty;
        public double EnergyOutput { get; set; } = 0;
        public string TimeZone { get; set; } = "Australia/Sydney";
    }
}