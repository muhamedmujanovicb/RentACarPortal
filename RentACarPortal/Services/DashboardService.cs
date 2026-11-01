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

        //no idea what queries here, not checked yet
        private Company? GetCompany(string username) => _context.Companies.FirstOrDefault(c => c.Username == username);
        private IQueryable<Contract> ContractsOwnedBy(int companyId) =>
        _context.Contracts.Where(c => c.CompanyId == companyId || 
        (c.CompanyId == null && _context.Vehicles.Any(v => v.CompanyId == companyId && v.RegisterNumberOfVehicle == c.RegisterNumberOfVehicle)));

        //gets pending contracts and eliminates expired ones
        public List<Contract> GetPendingContractsAndProcessExpirations(string loggedInUser)
        {
            //queries for a company vehicle by the company username
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
                //queries for expired requests on vehicles
                var expiredRegs = expiredVehicles.Select(v => v.RegisterNumberOfVehicle).ToList();

                //compares pending contracts and expired vehicles to check
                var expiredContracts = _context.Contracts
                    .Where(c => c.Status == ContractStatus.Pending && expiredRegs.Contains(c.RegisterNumberOfVehicle))
                    .ToList();

                //for every vehicle which has an expired contract it makes a new GUID 
                foreach (var vehicle in expiredVehicles)
                {
                    vehicle.Status = VehicleStatus.Available;
                    vehicle.HoldExpiresAt = null;
                    vehicle.Version = Guid.NewGuid();
                }

                //removes expired contracts
                if (expiredContracts.Any())
                {
                    _context.Contracts.RemoveRange(expiredContracts);
                }

                _context.SaveChanges();
            }

            // Fetch valid pending contracts for this company's vehicle registrations
            return _context.Contracts
                .Where(c => c.Status == ContractStatus.Pending && companyVehicleRegs.Contains(c.RegisterNumberOfVehicle))
                .OrderByDescending(c => c.Id)
                .ToList();
        }

        public List<Vehicle> GetCompanyFleetWithLookups(string loggedInUser)
        {
            //queries for all vehicles by vehicle make, model and type in the company which matches the username
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
            //queries for companies and their contracts which are linked by ids based on the username provided
            var user = _context.Companies
                .Include(u => u.Contracts)
                .FirstOrDefault(u => u.Username == loggedInUser);

            return user?.Contracts.ToList() ?? new List<Contract>();
        }

        public (Contract? Contract, Vehicle? Vehicle) GetContractRequestDetails(int id, string loggedInUser)
        {
            //gets company by the username
            var company = GetCompany(loggedInUser);
            if (company == null) return (null, null); //if not exist, fail fast

            //gets call contracts owned by the provided company by id
            var contract = ContractsOwnedBy(company.Id).FirstOrDefault(c => c.Id == id);
            if (contract == null) return (null, null); //if none exist by id, fail fast

            //queries for all vehicles with the vehicle make and model id as the vehicle with the matching register number
            var vehicle = _context.Vehicles
                .Include(v => v.VehicleMake)
                .Include(v => v.VehicleModel)
                .FirstOrDefault(v => v.RegisterNumberOfVehicle == contract.RegisterNumberOfVehicle);

            return (contract, vehicle);
        }

        public bool UpdateContractStatus(int id, string newStatus, string loggedInUser)
        {
            //if the contract is not completed and the new status is not cancelled, return false aka fail fast
            if (newStatus != ContractStatus.Completed && newStatus != ContractStatus.Cancelled) return false;

            //checks if the company exists by username
            var company = GetCompany(loggedInUser);
            if (company == null) return false;

            //queries for contracts owned by the company by the company id, inlcudes all of the vehicles owned by the company by id
            //which have an active status
            var contract = ContractsOwnedBy(company.Id)
                .Include(c => c.Vehicle)
                .FirstOrDefault(c => c.Id == id && c.Status == ContractStatus.Active);
            //if there is none, fail fast
            if (contract == null) return false;

            //sets the status for the contract
            contract.Status = newStatus;

            //if the vehicle linked to the contract is not null
            if (contract.Vehicle != null)
            {
                //change vehicle status and the expireation date and set a new GUID
                contract.Vehicle.Status = VehicleStatus.Available;
                contract.Vehicle.HoldExpiresAt = null;
                contract.Vehicle.Version = Guid.NewGuid();
            }

            _context.SaveChanges();
            return true;
        }

        public bool AcceptContractRequest(int id, string loggedInUser)
        {
            //gets the company by the provided username
            var companyUser = GetCompany(loggedInUser);
            if (companyUser == null) return false; //if there is none fail fast

            //gets all contracts owned by the company by id which include the vehicle by it's id and if it's status is pending
            var contract = ContractsOwnedBy(companyUser.Id)
                .Include(c => c.Vehicle)
                .FirstOrDefault(c => c.Id == id && c.Status == ContractStatus.Pending);
            if (contract == null) return false;//if none match the query, fail fast

            contract.Status = ContractStatus.Active;
            contract.RentalStation = loggedInUser;
            contract.Remarks = "Created from approved contract request.";
            contract.CompanyId = companyUser.Id;

            if (contract.Vehicle != null)
            {
                contract.Vehicle.Status = VehicleStatus.Rented;
                contract.Vehicle.HoldExpiresAt = null;
                contract.Vehicle.Version = Guid.NewGuid();
            }

            _context.SaveChanges();
            return true;
        }

        //ENTIRE METHOD IS LIKE ACCEPT CONTRACT REQUEST IN THIS SERVICE
        public bool DeclineContractRequest(int id, string loggedInUser)
        {
            var companyUser = GetCompany(loggedInUser);
            if (companyUser == null) return false;

            var contract = ContractsOwnedBy(companyUser.Id)
                .Include(c => c.Vehicle)
                .FirstOrDefault(c => c.Id == id && c.Status == ContractStatus.Pending);
            if (contract == null) return false;

            if (contract.Vehicle != null)
            {
                contract.Vehicle.Status = VehicleStatus.Available;
                contract.Vehicle.HoldExpiresAt = null;
                contract.Vehicle.Version = Guid.NewGuid();
            }

            _context.Contracts.Remove(contract);
            _context.SaveChanges();
            return true;
        }

        public void CreateContract(Contract newContract, string loggedInUser)
        {
            //queries for the company name which is creating the contract
            var user = _context.Companies.FirstOrDefault(u => u.Username == loggedInUser);
            //if the company exists create the contract
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