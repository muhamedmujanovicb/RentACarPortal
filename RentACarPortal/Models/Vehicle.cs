namespace RentACarPortal.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }


        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public int NumberOfSeats { get; set; }
        public bool IsDiesel { get; set; }
        public double FuelEfficiency { get; set; }
        public double FuelTankSize { get; set; }
        public string TypeOfVehicle { get; set; }
        public bool HasChildrenSafety { get; set; }
        public string DriveTerrain { get; set; }
        public string BootSpace { get; set; }
        public string ACtype { get; set; }
        public bool HasNavigation { get; set; }




        public double DailyRate { get; set; }
        public bool HasInsurance { get; set; }
        public string Status { get; set; } = "Available";
        public string RegisterNumberOfVehicle { get; set; }

        public Vehicle() { }
    }
   
}
