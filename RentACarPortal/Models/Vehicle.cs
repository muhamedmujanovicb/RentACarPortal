using RentACarPortal.Data;
using System.ComponentModel.DataAnnotations;

namespace RentACarPortal.Models
{
    public class Vehicle
    {
        // IMPORTANT ATTRIBUTES
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        [ConcurrencyCheck]
        public Guid Version { get; set; } = Guid.NewGuid();

        // VEHICLE MAKE LINK
        public int VehicleMakeId { get; set; }
        public VehicleMake? VehicleMake { get; set; }

        // VEHICLE MODEL LINK
        public int VehicleModelId { get; set; }
        public VehicleModel? VehicleModel { get; set; }

        // VEHICLE SPECIFIC ATTRIBUTES
        public int Year { get; set; }
        public int NumberOfSeats { get; set; }
        public bool IsDiesel { get; set; }
        public double FuelEfficiency { get; set; }
        public double FuelTankSize { get; set; }

        // VEHICLE TYPE LINK
        public int VehicleTypeId { get; set; }
        public VehicleType? VehicleType { get; set; }

        //OTHER ATTRIBUTES
        public bool HasChildrenSafety { get; set; }
        public string DriveTerrain { get; set; } = string.Empty;
        public string BootSpace { get; set; } = string.Empty;
        public string ACtype { get; set; } = string.Empty;
        public bool HasNavigation { get; set; }
        public DateTime? HoldExpiresAt { get; set; }

        // VEHICLE BUSINESS ATTRIBUTES
        public double DailyRate { get; set; }
        public bool HasInsurance { get; set; }
        public string Status { get; set; } = "Available";
        public string RegisterNumberOfVehicle { get; set; } = string.Empty;

        public Vehicle() { }
    }
}