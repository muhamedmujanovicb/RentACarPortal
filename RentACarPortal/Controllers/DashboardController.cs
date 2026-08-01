using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;
using SQLitePCL;

namespace RentACarPortal.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Dashboard(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == loggedInUser);

            if (currentUser == null)
            {
                return View(new List<BookingContractRequest>());
            }

            var currentTime = DateTime.Now;

            var expiredVehicles = _context.Vehicles
                .Where(v => v.HoldExpiresAt != null && v.HoldExpiresAt <= currentTime && v.UserId == currentUser.Id)
                .ToList();

            if (expiredVehicles.Any())
            {
                var vehicleIds = expiredVehicles.Select(v => v.Id).ToList();

                var expiredRequests = _context.BookingContractRequests
                    .Where(r => vehicleIds.Contains(r.VehicleId))
                    .ToList();

                foreach (var vehicle in expiredVehicles)
                {
                    vehicle.Status = "Available";
                    vehicle.HoldExpiresAt = null;
                }

                if (expiredRequests.Any())
                {
                    _context.BookingContractRequests.RemoveRange(expiredRequests);
                }

                _context.SaveChanges();
            }

            var validRequests = _context.BookingContractRequests
                .Include(r => r.Vehicle)
                .Where(r => r.CompanyId == loggedInUser) 
                .OrderByDescending(r => r.Id)
                .ToList();

            return View(validRequests);
        }

        [HttpGet]
        public IActionResult FleetManager(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            return View("FleetManager");
        }

        [HttpGet]
        public IActionResult FleetOverview(string loggedInUser)
        {
            var user = _context.Users.Include(u => u.Vehicles).FirstOrDefault(u => u.Username == loggedInUser);
            
            ViewBag.Username = loggedInUser;
            return View(user?.Vehicles.ToList()??new List<Vehicle>());
        }

        [HttpGet]
        public IActionResult ContractCreator(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            return View("ContractCreator");
        }

        [HttpGet]
        public IActionResult ContractHistory(string loggedInUser)
        {
            var user = _context.Users.Include(u => u.Contracts).FirstOrDefault(u => u.Username == loggedInUser);

            ViewBag.Username = loggedInUser;
            return View(user?.Contracts.ToList() ?? new List<Contract>());
        }

        [HttpGet]
        public IActionResult ContractRequestDetails(int id, string loggedInUser)
        {
            ViewBag.Username = loggedInUser;

            var request = _context.BookingContractRequests
                .Include(r => r.Vehicle)
                .FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return View("ContractRequestDetails", request);
        }
    }
}
