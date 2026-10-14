using Microsoft.EntityFrameworkCore;
using RentACarPortal.Models;

namespace RentACarPortal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<VehicleMake> VehicleMake { get; set; }
        public DbSet<VehicleModel> VehicleModel { get; set; }
        public DbSet<VehicleType> VehicleType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Client)
                .WithMany(u => u.Contracts)
                .HasForeignKey(c => c.ClientId);

            VehicleMakeSeed.SeedMakes(modelBuilder);
            VehicleModelSeed.SeedModels(modelBuilder);
            VehicleTypeSeed.SeedVehicleTypes(modelBuilder);
        }
    }
}
