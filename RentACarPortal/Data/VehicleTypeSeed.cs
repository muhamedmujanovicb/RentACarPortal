using Microsoft.EntityFrameworkCore;
using RentACarPortal.Models;

namespace RentACarPortal.Data
{
    public class VehicleTypeSeed
    {
        public static void SeedVehicleTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleType>().HasData(
                new VehicleType { Id = 1, Name = "Limuzina / Sedan" },
                new VehicleType { Id = 2, Name = "Malo auto / Hatchback" },
                new VehicleType { Id = 3, Name = "Karavan" },
                new VehicleType { Id = 4, Name = "SUV / Terenac" },
                new VehicleType { Id = 5, Name = "Monovolumen / Minivan" },
                new VehicleType { Id = 6, Name = "Kombi" },
                new VehicleType { Id = 7, Name = "Kabriolet" },
                new VehicleType { Id = 8, Name = "Coupe" }
            );
        }
    }
}
