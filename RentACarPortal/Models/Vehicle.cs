namespace RentACarPortal.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public double DailyRate { get; set; }
        public bool HasInsurance { get; set; }
        public string Status { get; set; } = "Available";
        public string RegisterNumberOfVehicle { get; set; }
    }
   
}
