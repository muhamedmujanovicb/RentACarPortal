using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public List<Contract> GetPendingContractsAndProcessExpirations(string loggedInUser)
        {
            var currentUser = _context.Companies
                .Include(u => u.Vehicles)
                .FirstOrDefault(u => u.Username == loggedInUser);

            if (currentUser == null || currentUser.Vehicles == null)
            {
                return new List<Contract>();
            }

            var currentTime = DateTime.Now;
            var companyVehicleRegs = currentUser.Vehicles.Select(v => v.RegisterNumberOfVehicle).ToList();

            // Check for expired vehicle holds and handle rollbacks
            var expiredVehicles = currentUser.Vehicles
                .Where(v => v.HoldExpiresAt != null && v.HoldExpiresAt <= currentTime)
                .ToList();

            if (expiredVehicles.Any())
            {
                var expiredRegs = expiredVehicles.Select(v => v.RegisterNumberOfVehicle).ToList();

                var expiredContracts = _context.Contracts
                    .Where(c => c.Status == "Pending" && expiredRegs.Contains(c.RegisterNumberOfVehicle))
                    .ToList();

                foreach (var vehicle in expiredVehicles)
                {
                    vehicle.Status = "Available";
                    vehicle.HoldExpiresAt = null;
                }

                if (expiredContracts.Any())
                {
                    _context.Contracts.RemoveRange(expiredContracts);
                }

                _context.SaveChanges();
            }

            // Fetch valid pending contracts for this company's vehicle registrations
            return _context.Contracts
                .Where(c => c.Status == "Pending" && companyVehicleRegs.Contains(c.RegisterNumberOfVehicle))
                .OrderByDescending(c => c.Id)
                .ToList();
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

        public List<Contract> GetCompanyContracts(string loggedInUser)
        {
            var user = _context.Companies
                .Include(u => u.Contracts)
                .FirstOrDefault(u => u.Username == loggedInUser);

            return user?.Contracts.ToList() ?? new List<Contract>();
        }

        public (Contract? Contract, Vehicle? Vehicle) GetContractRequestDetails(int id)
        {
            var contract = _context.Contracts.FirstOrDefault(c => c.Id == id);
            if (contract == null)
            {
                return (null, null);
            }

            var vehicle = _context.Vehicles
                .Include(v => v.VehicleMake)
                .Include(v => v.VehicleModel)
                .FirstOrDefault(v => v.RegisterNumberOfVehicle == contract.RegisterNumberOfVehicle);

            return (contract, vehicle);
        }

        public List<Contract> GetContractHistory()
        {
            return _context.Contracts
                .Include(c => c.Client)
                .ToList();
        }

        public void UpdateContractStatus(int id, string newStatus)
        {
            var contract = _context.Contracts.Find(id);
            if (contract != null)
            {
                contract.Status = newStatus;
                _context.SaveChanges();
            }
        }

        public Contract? GetContractDetailsWithLookups(int id)
        {
            return _context.Contracts
                .Include(c => c.Vehicle)
                    .ThenInclude(v => v.VehicleMake)
                .Include(c => c.Vehicle)
                    .ThenInclude(v => v.VehicleModel)
                .Include(c => c.Client)
                .FirstOrDefault(c => c.Id == id);
        }

        public bool AcceptContractRequest(int id, string loggedInUser)
        {
            var contract = _context.Contracts
                .Include(c => c.Vehicle)
                .FirstOrDefault(c => c.Id == id);

            if (contract == null) return false;

            var companyUser = _context.Companies.FirstOrDefault(c => c.Username == loggedInUser);
            if (companyUser == null) return false;

            contract.Status = "Active";
            contract.RentalStation = loggedInUser;
            contract.Remarks = "Created from approved contract request.";
            contract.CompanyId = companyUser.Id;

            if (contract.Vehicle != null)
            {
                contract.Vehicle.Status = "Rented";
                contract.Vehicle.HoldExpiresAt = null;
            }

            _context.SaveChanges();
            return true;
        }

        public bool DeclineContractRequest(int id, string loggedInUser)
        {
            var contract = _context.Contracts
                .Include(c => c.Vehicle)
                .FirstOrDefault(c => c.Id == id);

            if (contract == null) return false;

            var companyUser = _context.Companies.FirstOrDefault(c => c.Username == loggedInUser);
            if (companyUser == null) return false;

            if (contract.Vehicle != null)
            {
                contract.Vehicle.Status = "Available";
                contract.Vehicle.HoldExpiresAt = null;
            }

            _context.Contracts.Remove(contract);
            _context.SaveChanges();
            return true;
        }
    }
}

