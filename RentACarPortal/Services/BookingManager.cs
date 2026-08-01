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

        public void CreateBookingRequest(
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
            var contractRequest = new BookingContractRequest
            {
                VehicleId = vehicleId,
                Username = loggedInUser,
                CompanyId = selectedCompany,
                DriverFullName = driverFullName,
                DateOfBirth = dateOfBirth,
                PersonalIdNumber = personalIdNumber,
                Telephone = telephone,
                Address = address,
                DrivingLicenseNumber = drivingLicenseNumber,
                PassportNumber = passportNumber,
                PlaceOfIssue = placeOfIssue,
                DateOfIssue = dateOfIssue,
                Notes = notes,
                RentStartDate = rentStartDate,
                RentLenght = rentLength,
                TotalPrice = totalPrice
            };

            _context.BookingContractRequests.Add(contractRequest);
            _context.SaveChanges();
        }
    }
}
