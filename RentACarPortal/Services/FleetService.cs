using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Services
{
    public class FleetService
    {
        private readonly AppDbContext _context;

        public FleetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vehicle>> GetAvailableVehiclesByCompanyAsync(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
            {
                return new List<Vehicle>();
            }

            var company = await _context.Companies
                .Include(c => c.Vehicles)
                    .ThenInclude(v => v.VehicleMake)
                .Include(c => c.Vehicles)
                    .ThenInclude(v => v.VehicleModel)
                .FirstOrDefaultAsync(c => c.Username.ToLower() == companyName.ToLower());

            if (company == null || company.Vehicles == null)
            {
                return new List<Vehicle>();
            }

            return company.Vehicles
                .Where(v => v.Status != "Pending")
                .ToList();
        }

        public void AddVehicleToFleet(Vehicle newVehicle, string loggedInUser)
        {
            var user = _context.Companies.FirstOrDefault(u => u.Username == loggedInUser);
            if (user != null)
            {
                newVehicle.CompanyId = user.Id;
                _context.Vehicles.Add(newVehicle);
                _context.SaveChanges();
            }
        }

        public List<Vehicle> GetCompanyFleetWithLookups(string loggedInUser)
        {
            var user = _context.Companies
                .Include(u => u.Vehicles)
                    .ThenInclude(v => v.VehicleMake)
                .Include(u => u.Vehicles)
                    .ThenInclude(v => v.VehicleModel)
                .FirstOrDefault(u => u.Username == loggedInUser);

            return user?.Vehicles.ToList() ?? new List<Vehicle>();
        }

        public void DeleteVehicle(int id)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                _context.SaveChanges();
            }
        }

        //Helper methods
        public List<VehicleMake> GetAllMakes() => _context.VehicleMake.ToList();
        public List<VehicleModel> GetAllModels() => _context.VehicleModel.ToList();
        public List<VehicleType> GetAllVehicleTypes() => _context.VehicleType.ToList();
    }
}

