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

            return View();
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
            Console.WriteLine($"DEBUG: Looking for user: '{loggedInUser}'");

            var user = _context.Users.Include(u => u.Vehicles).FirstOrDefault(u => u.Username == loggedInUser);
            if (user == null)
            {
                Console.WriteLine("DEBUG: User not found!");
            }
            else
            {
                Console.WriteLine($"DEBUG: Found user {user.Username} with {user.Vehicles.Count} vehicles");
            }

            ViewBag.Username = loggedInUser;
            return View(user?.Vehicles.ToList()??new List<Vehicle>());
        }
    }
}
