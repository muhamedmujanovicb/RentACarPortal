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
                .Where(v => v.Status != VehicleStatus.Pending)
                .ToList();
        }

        public string? AddVehicleToFleet(Vehicle newVehicle, string loggedInUser)
        {
            var user = _context.Companies.FirstOrDefault(u => u.Username == loggedInUser);
            if (user == null) return "Your company account could not be found. Please log in again.";

            if (_context.Vehicles.Any(v => v.RegisterNumberOfVehicle == newVehicle.RegisterNumberOfVehicle))
                return "A vehicle with that registration number already exists.";

            newVehicle.CompanyId = user.Id;
            _context.Vehicles.Add(newVehicle);
            _context.SaveChanges();
            return null;
        }

        public bool DeleteVehicle(int id, string loggedInUser)
        {
            var company = _context.Companies.FirstOrDefault(c => c.Username == loggedInUser);
            if (company == null) return false;

            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == id && v.CompanyId == company.Id);
            if (vehicle == null) return false; 

            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
            return true;
        }

        //Helper methods
        public List<VehicleMake> GetAllMakes() => _context.VehicleMake.ToList();
        public List<VehicleModel> GetAllModels() => _context.VehicleModel.ToList();
        public List<VehicleType> GetAllVehicleTypes() => _context.VehicleType.ToList();
        public List<Company> GetAllCompanies() => _context.Companies.ToList();
        public Vehicle? GetVehicleById(int id) => _context.Vehicles.Find(id);
        public List<string> GetAllClientUsernames() => _context.Clients.Select(c => c.Username).ToList();
    }
}