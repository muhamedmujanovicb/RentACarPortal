using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class UserDashboardController : Controller
    {
        private readonly AppDbContext _context;
        public UserDashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult UserDashboard(string loggedInUser = "Admin")
        {
            ViewBag.Username = loggedInUser;

            return View();
        }

        [HttpGet]
        public IActionResult Recommendor()
        {
            return View("Recommendor");
        }

        [HttpGet]
        public IActionResult BookingCompanies(string loggedInUser)
        {
            ViewBag.Username = loggedInUser ?? "Admin";

            var companies = _context.Users.Where(u => u.IsAdmin).ToList();

            return View("BookingCompanies", companies);
        }

        [HttpGet]
        public IActionResult BookingFleet(string loggedInUser, string companyName)
        {
            ViewBag.Username = loggedInUser ?? "Admin";

            return View("BookingFleet");
        }
    }
}
