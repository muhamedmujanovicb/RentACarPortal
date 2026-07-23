using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class BookingFleetController : Controller
    {
        private readonly AppDbContext _context;

        public BookingFleetController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult BookingFleet(string loggedInUser, string companyName)
        {
            ViewBag.Username = loggedInUser;
            ViewBag.SelectedCompany = companyName;

            // 1. Find the company safely (ignoring case)
            var company = _context.Users
                .Include(u => u.Vehicles)
                .FirstOrDefault(u => u.Username.ToLower() == companyName.ToLower());

            // 2. Fallback: If no vehicles match "Available", let's pull all vehicles for this company to test
            var availableVehicles = company?.Vehicles
                .ToList() ?? new List<Vehicle>();

            return View("~/Views/UserDashboard/BookingFleet.cshtml", availableVehicles);
        }
    }
}
