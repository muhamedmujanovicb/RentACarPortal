using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace RentACarPortal.Models
{
    public class VehicleRecommendation
    {
        public int seats { get; set; }
        public double fuelConsumption { get; set; }
        public double dailyRate { get; set; }
        public bool childrenSafety { get; set; }
        public string driveTerrain { get; set; }
        public string bootSpace { get; set; }
        public bool hasGps { get; set; }
        public string acType { get; set; }

        public string terrainType { get; set; }
        public int tripDuration { get; set; }
        public int passengerCount { get; set; }
        public bool hasChildren { get; set; }
        public bool hasElderly { get; set; }
        public int distance { get; set; }
        public int budget { get; set; }
        public string weather { get; set; }

        public double fuelCost { get; set; }
        public double highwayCosts { get; set; }
    }
}
