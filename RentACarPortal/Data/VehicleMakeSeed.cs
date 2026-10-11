using Microsoft.EntityFrameworkCore;
using RentACarPortal.Models;
using System.Xml.Linq;

namespace RentACarPortal.Data
{
    public static class VehicleMakeSeed
    {
        public static void SeedMakes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleMake>().HasData(
    new VehicleMake { Id = 1, Name = "Audi" }, 
    new VehicleMake { Id = 2, Name = "BMW" }, 
    new VehicleMake { Id = 3, Name = "Citroen" }, 
    new VehicleMake { Id = 4, Name = "Fiat" },
    new VehicleMake { Id = 5, Name = "Ford" }, 
    new VehicleMake { Id = 6, Name = "Mercedes-Benz" }, 
    new VehicleMake { Id = 7, Name = "Opel" },
    new VehicleMake { Id = 8, Name = "Peugeot" }, 
    new VehicleMake { Id = 9, Name = "Renault" }, 
    new VehicleMake { Id = 10, Name = "Škoda" }, 
    new VehicleMake { Id = 11, Name = "Toyota" }, 
    new VehicleMake { Id = 12, Name = "Volkswagen" } 
);
        }
    }
}