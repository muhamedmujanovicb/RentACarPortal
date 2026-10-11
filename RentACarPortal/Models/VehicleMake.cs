namespace RentACarPortal.Models
{
    public class VehicleMake
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Vehicle> Models { get; set; } = new List<Vehicle>();
    }
}
