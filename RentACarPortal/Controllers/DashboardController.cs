using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

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

            // Find the current user/company managing the fleet
            var currentUser = _context.Companies
                .Include(u => u.Vehicles)
                .FirstOrDefault(u => u.Username == loggedInUser);

            if (currentUser == null || currentUser.Vehicles == null)
            {
                return View(new List<Contract>());
            }

            var currentTime = DateTime.Now;

            // Get registration numbers belonging to this company's vehicles
            var companyVehicleRegs = currentUser.Vehicles.Select(v => v.RegisterNumberOfVehicle).ToList();

            // Check for expired vehicle holds
            var expiredVehicles = currentUser.Vehicles
                .Where(v => v.HoldExpiresAt != null && v.HoldExpiresAt <= currentTime)
                .ToList();

            if (expiredVehicles.Any())
            {
                var expiredRegs = expiredVehicles.Select(v => v.RegisterNumberOfVehicle).ToList();

                // Find pending contracts associated with expired vehicles
                var expiredContracts = _context.Contracts
                    .Where(c => c.Status == "Pending" && expiredRegs.Contains(c.RegisterNumberOfVehicle))
                    .ToList();

                foreach (var vehicle in expiredVehicles)
                {
                    vehicle.Status = "Available";
                    vehicle.HoldExpiresAt = null;
                }

                if (expiredContracts.Any())
                {
                    _context.Contracts.RemoveRange(expiredContracts);
                }

                _context.SaveChanges();
            }

            // Fetch pending contracts for this company's vehicle registrations
            var validContracts = _context.Contracts
                .Where(c => c.Status == "Pending" && companyVehicleRegs.Contains(c.RegisterNumberOfVehicle))
                .OrderByDescending(c => c.Id)
                .ToList();

            return View(validContracts);
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
            var user = _context.Companies.Include(u => u.Vehicles).FirstOrDefault(u => u.Username == loggedInUser);
            
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
            var user = _context.Companies.Include(u => u.Contracts).FirstOrDefault(u => u.Username == loggedInUser);

            ViewBag.Username = loggedInUser;
            return View(user?.Contracts.ToList() ?? new List<Contract>());
        }

        [HttpGet]
        public IActionResult ContractRequestDetails(int id, string loggedInUser)
        {
            ViewBag.Username = loggedInUser;

            var contract = _context.Contracts
                .FirstOrDefault(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            // Pass the associated vehicle along via ViewBag since Contract links via RegisterNumberOfVehicle
            ViewBag.Vehicle = _context.Vehicles
                .FirstOrDefault(v => v.RegisterNumberOfVehicle == contract.RegisterNumberOfVehicle);

            return View("ContractRequestDetails", contract);
        }

        [HttpGet]
        public IActionResult Statistics(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;

            return RedirectToAction("Index", "Statistics", new { loggedInUser });
        }
    }
}
