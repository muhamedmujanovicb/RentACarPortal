using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Services
{
    public class BookingManager
    {
        private readonly AppDbContext _context;

        public BookingManager(AppDbContext context)
        {
            _context = context;
        }

        public bool ProcessBookingRequest(int vehicleId, string loggedInUser)
        {
            //fail fast check
            if (!_context.Clients.Any(c => c.Username == loggedInUser)) return false;
            var vehicle = _context.Vehicles.Find(vehicleId);
            if (vehicle == null || vehicle.Status != VehicleStatus.Available) return false;

            vehicle.Status = VehicleStatus.Pending;
            vehicle.HoldExpiresAt = DateTime.Now.AddMinutes(10);
            vehicle.Version = Guid.NewGuid();

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public ContractEmailData? CreateContractRequest(
            int vehicleId,
            string loggedInUser,
            string selectedCompany,
            string driverFullName,
            DateOnly dateOfBirth,
            string personalIdNumber,
            string telephone,
            string address,
            string drivingLicenseNumber,
            string passportNumber,
            string placeOfIssue,
            DateOnly dateOfIssue,
            string notes,
            DateOnly rentStartDate,
            int rentLength,
            double totalPrice
        )
        {
            var user = _context.Clients.FirstOrDefault(u => u.Username == loggedInUser);
            if (user == null) return null;

            // Eager loads the VehicleModel to quickly access it's name string
            var vehicle = _context.Vehicles
                .Include(v => v.VehicleModel)
                .Include(v => v.Company)
                .FirstOrDefault(v => v.Id == vehicleId);

            var newContract = new Contract
            {
                ClientId = user.Id,
                RegisterNumberOfVehicle = vehicle != null ? vehicle.RegisterNumberOfVehicle : vehicleId.ToString(),
                TypeOfVehicle = vehicle?.VehicleModel?.Name ?? "Unknown",
                RentDriver = driverFullName,
                Address = address,
                Telephone = telephone,
                PassportNumber = passportNumber,
                PlaceOfIssue = placeOfIssue,
                DateOfIssue = dateOfIssue,
                DateOfBirth = dateOfBirth,
                PersonalNumber = personalIdNumber,
                DrivingLicenseNumber = drivingLicenseNumber,
                RentalStartDate = rentStartDate,
                RentalEndDate = rentStartDate.AddDays(rentLength),
                RentalStartPlace = placeOfIssue,
                RentalEndPlace = placeOfIssue,
                Comment = notes,
                Status = ContractStatus.Pending,
                TotalPrice = totalPrice,
                VehicleId = vehicle?.Id,
            };

            _context.Contracts.Add(newContract);
            _context.SaveChanges();

            return new ContractEmailData(
                ClientEmail: user.Email,
                ClientUsername: user.Username,
                CompanyEmail: vehicle?.Company?.Email ?? string.Empty,
                CompanyName: vehicle?.Company?.Username ?? string.Empty,
                VehicleName: newContract.TypeOfVehicle,
                RegisterNumber: newContract.RegisterNumberOfVehicle,
                StartDate: newContract.RentalStartDate,
                EndDate: newContract.RentalEndDate,
                TotalPrice: newContract.TotalPrice,
                DriverName: newContract.RentDriver,
                Comment: newContract.Comment,
                Remarks: string.Empty);
        }
    }
}