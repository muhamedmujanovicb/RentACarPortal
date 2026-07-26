using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class BookingContractRequestController : Controller
    {
        private readonly BookingManager _bookingManager;
        public BookingContractRequestController(BookingManager bookingManager)
        {
            _bookingManager = bookingManager;
        }

        [HttpGet]
        public IActionResult BookingContractRequest(string loggedInUser, string selectedCompany, int vehicleId)
        {
            ViewBag.Username = loggedInUser;
            ViewBag.SelectedCompany = selectedCompany;
            ViewBag.VehicleId = vehicleId;

            return View("~/Views/UserDashboard/BookingContractRequest.cshtml");
        }

        [HttpPost]
        public IActionResult SubmitRequest(
            int vehicleId, 
            string loggedInUser, 
            string selectedCompany, 
            string driverFullName, 
            DateTime dateOfBirth, 
            string personalIdNumber, 
            string telephone, 
            string address, 
            string drivingLicenseNumber, 
            string passportNumber, 
            string placeOfIssue, 
            DateTime dateOfIssue, 
            string notes)
        {
            _bookingManager.ProcessBookingRequest(vehicleId, loggedInUser);

            _bookingManager.CreateBookingRequest(
                vehicleId,
                loggedInUser,
                selectedCompany,
                driverFullName,
                DateOnly.FromDateTime(dateOfBirth),
                personalIdNumber,
                telephone,
                address,
                drivingLicenseNumber,
                passportNumber,
                placeOfIssue,
                DateOnly.FromDateTime(dateOfIssue),
                notes);

                return RedirectToAction("UserDashboard", "UserDashboard", new { loggedInUser = loggedInUser });
        }
    }
}
