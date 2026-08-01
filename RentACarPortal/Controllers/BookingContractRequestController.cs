using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Services;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class BookingContractRequestController : Controller
    {
        private readonly BookingManager _bookingManager;
        private readonly AppDbContext _context;
        public BookingContractRequestController(BookingManager bookingManager, AppDbContext context)
        {
            _bookingManager = bookingManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult BookingContractRequest(string loggedInUser, string selectedCompany, int vehicleId)
        {
            ViewBag.Username = loggedInUser;
            ViewBag.SelectedCompany = selectedCompany;
            ViewBag.VehicleId = vehicleId;

            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
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
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            double totalPrice = vehicle != null ? (vehicle.DailyRate * rentLength) : 0;

            _bookingManager.ProcessBookingRequest(vehicleId, loggedInUser);

            _bookingManager.CreateBookingRequest(
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
                (double)totalPrice
                );

                return RedirectToAction("UserDashboard", "UserDashboard", new { loggedInUser = loggedInUser });
        }
    }
}
