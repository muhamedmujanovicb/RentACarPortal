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
            //checks if the company name is null
            if (string.IsNullOrWhiteSpace(companyName))
            {
                return new List<Vehicle>();
            }

            //queries for all vehicles -> vehicle makes -> vehicle models from the company which was selected 
            var company = await _context.Companies
                .Include(c => c.Vehicles)
                    .ThenInclude(v => v.VehicleMake)
                .Include(c => c.Vehicles)
                    .ThenInclude(v => v.VehicleModel)
                .FirstOrDefaultAsync(c => c.Username.ToLower() == companyName.ToLower());

            //checks if the company is null or it has no vehicles
            if (company == null || company.Vehicles == null)
            {
                return new List<Vehicle>();
            }

            //queries for remaining vehicles which status is not pending, which means the vehicles are available for rent
            return company.Vehicles
                .Where(v => v.Status != VehicleStatus.Pending)
                .ToList();
        }

        public string? AddVehicleToFleet(Vehicle newVehicle, string loggedInUser)
        {
            //queries for company which match the username
            var user = _context.Companies.FirstOrDefault(u => u.Username == loggedInUser);
            if (user == null) return "Your company account could not be found. Please log in again.";

            //if any vehicles exist with the register number of vehicle of the new vehicle throw an error
            if (_context.Vehicles.Any(v => v.RegisterNumberOfVehicle == newVehicle.RegisterNumberOfVehicle))
                return "A vehicle with that registration number already exists.";

            //else add vehicle
            newVehicle.CompanyId = user.Id;
            _context.Vehicles.Add(newVehicle);
            _context.SaveChanges();
            return null;
        }

        public bool DeleteVehicle(int id, string loggedInUser)
        {
            //queries for company which matches the provided username
            var company = _context.Companies.FirstOrDefault(c => c.Username == loggedInUser);
            if (company == null) return false; //if it doesn't exist, fail fast

            //queries for vehicle which matches the company which owns it by id
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == id && v.CompanyId == company.Id);
            if (vehicle == null) return false; //if none match, fail fast

            //else remove
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
    }
}