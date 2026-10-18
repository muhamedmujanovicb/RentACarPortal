using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACarPortal.Controllers
{
    [Authorize(Roles = "Client")]

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
        public IActionResult BookingContractRequest(string selectedCompany, int vehicleId)
        {
            var loggedInUser = User.Identity!.Name!;
            ViewBag.Username = loggedInUser;
            ViewBag.SelectedCompany = selectedCompany;
            ViewBag.VehicleId = vehicleId;

            var vehicle = _fleetService.GetVehicleById(vehicleId);
            ViewBag.VehiclePrice = vehicle != null ? vehicle.DailyRate : 0.00;

            return View();
        }

        [HttpPost]
        public IActionResult SubmitRequest(
            int vehicleId,
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
            var loggedInUser = User.Identity!.Name!;
            var vehicle = _fleetService.GetVehicleById(vehicleId);
            double totalPrice = vehicle != null ? vehicle.DailyRate * rentLength : 0;

            if (!_bookingManager.ProcessBookingRequest(vehicleId, loggedInUser))
            {
                TempData["BookingError"] = "Sorry, that vehicle is no longer available.";
                return RedirectToAction("BookingCompanies", "UserDashboard");
            }
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