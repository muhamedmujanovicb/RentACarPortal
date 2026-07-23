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

            var company = _context.Users
                .Include(u => u.Vehicles)
                .FirstOrDefault(u => u.Username.ToLower() == companyName.ToLower());

            var availableVehicles = company?.Vehicles
                .ToList() ?? new List<Vehicle>();

            return View("~/Views/UserDashboard/BookingFleet.cshtml", availableVehicles);
        }
    }
}
