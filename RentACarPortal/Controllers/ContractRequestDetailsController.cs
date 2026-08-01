using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class ContractRequestDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public ContractRequestDetailsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ContractRequestDetails(int id, string loggedInUser)
        {
            ViewBag.Username = loggedInUser;

            var request = _context.BookingContractRequests
                .Include(b => b.Vehicle)
                .FirstOrDefault(b => b.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        [HttpPost]
        public IActionResult AcceptRequest(int id, string loggedInUser)
        {
            var request = _context.BookingContractRequests
                .Include(r => r.Vehicle)
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            var adminUser = _context.Users.FirstOrDefault(u => u.Username == loggedInUser);
            if (adminUser == null)
            {
                return Unauthorized();
            }

            DateOnly rentalEndDate = request.RentStartDate.AddDays(request.RentLenght);

            int.TryParse(request.PersonalIdNumber, out int parsedPersonalNumber);
            int.TryParse(request.DrivingLicenseNumber, out int parsedLicenseNumber);

            var newContract = new Contract
            {
                UserId = adminUser.Id,
                Status = "Active",
                DriverFullName = request.DriverFullName,
                RentalStation = loggedInUser,
                TypeOfVehicle = request.Vehicle != null ? $"{request.Vehicle.Make} {request.Vehicle.Model}" : "Unknown",
                RegisterNumberOfVehicle = request.Vehicle?.RegisterNumberOfVehicle ?? "N/A",
                RentDriver = request.DriverFullName,
                Address = request.Address,
                Telephone = request.Telephone,
                PassportNumber = request.PassportNumber,
                PlaceOfIssue = request.PlaceOfIssue,
                DateOfIssue = request.DateOfIssue,
                DateOfBirth = request.DateOfBirth,
                PersonalNumber = parsedPersonalNumber,
                DrivingLicenseNumber = parsedLicenseNumber,
                RentalStartDate = request.RentStartDate,
                RentalStartTime = new TimeOnly(8, 0),
                RentalStartPlace = request.PlaceOfIssue,
                RentalEndDate = rentalEndDate,
                RentalEndTime = new TimeOnly(18, 0),
                RentalEndPlace = request.PlaceOfIssue,
                Insurance = request.Vehicle?.HasInsurance ?? false,
                FuelRecieved = "Full",
                FuelReturned = "Full",
                FullTankSizeLiquid = request.Vehicle?.FuelTankSize ?? 50.0,
                Deposit = 100.00,
                Comment = request.Notes,
                Remarks = "Created from approved contract request."
            };

            _context.Contracts.Add(newContract);

            if (request.Vehicle != null)
            {
                request.Vehicle.Status = "Rented";
                request.Vehicle.HoldExpiresAt = null;
            }
            _context.BookingContractRequests.Remove(request);

            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }

        [HttpPost]
        public IActionResult DeclineRequest(int id, string loggedInUser)
        {
            var request = _context.BookingContractRequests
                .Include(r => r.Vehicle)
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.Vehicle != null)
            {
                request.Vehicle.Status = "Available";
                request.Vehicle.HoldExpiresAt = null;
            }

            _context.BookingContractRequests.Remove(request);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}
