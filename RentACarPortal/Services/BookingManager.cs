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
            if (!_context.Clients.Any(c => c.Username == loggedInUser)) return false;
            var vehicle = _context.Vehicles.Find(vehicleId);
            if (vehicle == null || vehicle.Status != "Available") return false;

            vehicle.Status = "Pending";
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

        public bool CreateContractRequest(
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
            if (user == null) return false;

            // Eager load VehicleModel so we can access its Name string
            var vehicle = _context.Vehicles
                .Include(v => v.VehicleModel)
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
                Status = "Pending",
                TotalPrice = totalPrice,
                VehicleId = vehicle?.Id,
            };

            _context.Contracts.Add(newContract);
            _context.SaveChanges();
            return true;
        }
    }
}