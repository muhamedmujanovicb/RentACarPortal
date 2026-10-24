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

        private Company? GetCompany(string username) => _context.Companies.FirstOrDefault(c => c.Username == username);
        private IQueryable<Contract> ContractsOwnedBy(int companyId) =>
        _context.Contracts.Where(c => c.CompanyId == companyId || 
        (c.CompanyId == null && _context.Vehicles.Any(v => v.CompanyId == companyId && v.RegisterNumberOfVehicle == c.RegisterNumberOfVehicle)));

        public List<Contract> GetPendingContractsAndProcessExpirations(string loggedInUser)
        {
            var currentUser = _context.Companies
                .Include(u => u.Vehicles).ThenInclude(v => v.VehicleMake)
                .Include(u => u.Vehicles).ThenInclude(v => v.VehicleModel)
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
                    vehicle.Version = Guid.NewGuid();
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
                .Include(u => u.Vehicles)
                    .ThenInclude(v => v.VehicleType) 
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

        public (Contract? Contract, Vehicle? Vehicle) GetContractRequestDetails(int id, string loggedInUser)
        {
            var company = GetCompany(loggedInUser);
            if (company == null) return (null, null);

            var contract = ContractsOwnedBy(company.Id).FirstOrDefault(c => c.Id == id);
            if (contract == null) return (null, null);

            var vehicle = _context.Vehicles
                .Include(v => v.VehicleMake)
                .Include(v => v.VehicleModel)
                .FirstOrDefault(v => v.RegisterNumberOfVehicle == contract.RegisterNumberOfVehicle);

            return (contract, vehicle);
        }

        public bool UpdateContractStatus(int id, string newStatus, string loggedInUser)
        {
            if (newStatus != "Completed" && newStatus != "Cancelled") return false;

            var company = GetCompany(loggedInUser);
            if (company == null) return false;

            var contract = ContractsOwnedBy(company.Id).FirstOrDefault(c => c.Id == id);
            if (contract == null) return false;

            contract.Status = newStatus;
            _context.SaveChanges();
            return true;
        }

        public bool AcceptContractRequest(int id, string loggedInUser)
        {
            var companyUser = GetCompany(loggedInUser);
            if (companyUser == null) return false;

            var contract = ContractsOwnedBy(companyUser.Id)
                .Include(c => c.Vehicle)
                .FirstOrDefault(c => c.Id == id && c.Status == "Pending");
            if (contract == null) return false;

            contract.Status = "Active";
            contract.RentalStation = loggedInUser;
            contract.Remarks = "Created from approved contract request.";
            contract.CompanyId = companyUser.Id;

            if (contract.Vehicle != null)
            {
                contract.Vehicle.Status = "Rented";
                contract.Vehicle.HoldExpiresAt = null;
                contract.Vehicle.Version = Guid.NewGuid();
            }

            _context.SaveChanges();
            return true;
        }

        public bool DeclineContractRequest(int id, string loggedInUser)
        {
            var companyUser = GetCompany(loggedInUser);
            if (companyUser == null) return false;

            var contract = ContractsOwnedBy(companyUser.Id)
                .Include(c => c.Vehicle)
                .FirstOrDefault(c => c.Id == id && c.Status == "Pending");
            if (contract == null) return false;

            if (contract.Vehicle != null)
            {
                contract.Vehicle.Status = "Available";
                contract.Vehicle.HoldExpiresAt = null;
                contract.Vehicle.Version = Guid.NewGuid();
            }

            _context.Contracts.Remove(contract);
            _context.SaveChanges();
            return true;
        }

        public void CreateContract(Contract newContract, string loggedInUser)
        {
            var user = _context.Companies.FirstOrDefault(u => u.Username == loggedInUser);
            if (user != null)
            {
                newContract.CompanyId = user.Id;
                newContract.RentalStation = loggedInUser;
                _context.Contracts.Add(newContract);
                _context.SaveChanges();
            }
        }


    }
}