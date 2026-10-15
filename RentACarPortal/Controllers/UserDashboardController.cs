using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class UserDashboardController : Controller
    {
        private readonly FleetService _fleetService;

        public UserDashboardController(FleetService fleetService)
        {
            _fleetService = fleetService;
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
            ViewBag.Companies = _fleetService.GetAllClientUsernames();
            return View("Recommendor");
        }

        [HttpGet]
        public IActionResult BookingCompanies(string loggedInUser)
        {
            ViewBag.Username = loggedInUser ?? "Admin";
            var companies = _fleetService.GetAllCompanies();
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