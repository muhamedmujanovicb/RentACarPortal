using System.Runtime.InteropServices.Marshalling;

namespace RentACarPortal.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int MakeId { get; set; }
        public VehicleMake? Make { get; set; }
    }
}
