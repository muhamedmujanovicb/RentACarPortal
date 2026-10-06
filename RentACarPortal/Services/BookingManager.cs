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

        public void ProcessBookingRequest(int vehicleId, string loggedInUser)
        {
            var vehicle = _context.Vehicles.Find(vehicleId);
            if (vehicle != null && vehicle.Status == "Available")
            {
                vehicle.Status = "Pending";
                vehicle.HoldExpiresAt = DateTime.Now.AddMinutes(10);
                _context.SaveChanges();
            }
        }

        public void CreateContractRequest(
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
            var vehicle = _context.Vehicles.Find(vehicleId);

            var newContract = new Contract
            {
                ClientId = user != null ? user.Id : 0,
                RegisterNumberOfVehicle = vehicle != null ? vehicle.RegisterNumberOfVehicle : vehicleId.ToString(),
                TypeOfVehicle = vehicle != null ? vehicle.Model : "Unknown",
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
                Status = "Pending"
            };

            _context.Contracts.Add(newContract);
            _context.SaveChanges();
        }
    }
}
