using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class BookingContractRequestController : Controller
    {
        private readonly BookingManager _bookingManager;
        private readonly FleetService _fleetService;

        public BookingContractRequestController(BookingManager bookingManager, FleetService fleetService)
        {
            _bookingManager = bookingManager;
            _fleetService = fleetService;
        }

        [HttpGet]
        public IActionResult BookingContractRequest(string loggedInUser, string selectedCompany, int vehicleId)
        {
            ViewBag.Username = loggedInUser;
            ViewBag.SelectedCompany = selectedCompany;
            ViewBag.VehicleId = vehicleId;

            var vehicle = _fleetService.GetVehicleById(vehicleId);
            ViewBag.VehiclePrice = vehicle != null ? vehicle.DailyRate : 0.00;

            return View("~/Views/UserDashboard/BookingContractRequest.cshtml");
        }

        [HttpPost]
        public IActionResult SubmitRequest(
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
            DateOnly rentStartDate,
            int rentLength,
            string notes
        )
        {
            var vehicle = _fleetService.GetVehicleById(vehicleId);
            double totalPrice = vehicle != null ? vehicle.DailyRate * rentLength : 0;

            _bookingManager.ProcessBookingRequest(vehicleId, loggedInUser);
            _bookingManager.CreateContractRequest(
                vehicleId,
                loggedInUser,
                selectedCompany,
                driverFullName,
                dateOfBirth,
                personalIdNumber,
                telephone,
                address,
                drivingLicenseNumber,
                passportNumber,
                placeOfIssue,
                dateOfIssue,
                notes,
                rentStartDate,
                rentLength,
                totalPrice
            );

            return RedirectToAction("UserDashboard", "UserDashboard", new { loggedInUser = loggedInUser });
        }
    }
}