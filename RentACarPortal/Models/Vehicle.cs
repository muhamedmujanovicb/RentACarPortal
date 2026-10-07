namespace RentACarPortal.Models
{
    public class Vehicle
    {
        //IMPORTANT ATTRIBUTES
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        //VEHICLE SPECIFIC ATTRIBUTES
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public int NumberOfSeats { get; set; }
        public bool IsDiesel { get; set; }
        public double FuelEfficiency { get; set; }
        public double FuelTankSize { get; set; }
        public string TypeOfVehicle { get; set; } = string.Empty;
        public bool HasChildrenSafety { get; set; }
        public string DriveTerrain { get; set; } = string.Empty;
        public string BootSpace { get; set; } = string.Empty;
        public string ACtype { get; set; } = string.Empty;
        public bool HasNavigation { get; set; }
        public DateTime? HoldExpiresAt { get; set; }

        //VEHICLE BUSINESS ATTRIBUTES
        public double DailyRate { get; set; }
        public bool HasInsurance { get; set; }
        public string Status { get; set; } = "Available";
        public string RegisterNumberOfVehicle { get; set; } = string.Empty;

        public Vehicle() { }
    }
   
}
