using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACarPortal.Controllers
{
    [Authorize(Roles = "Client")]

    public class UserDashboardController : Controller
    {
        private readonly FleetService _fleetService;

        public UserDashboardController(FleetService fleetService)
        {
            _fleetService = fleetService;
        }

        [HttpGet]
        public IActionResult UserDashboard()
        {
            var loggedInUser = User.Identity!.Name!;
            ViewBag.Username = loggedInUser;
            return View();
        }

        [HttpGet]
        public IActionResult BookingCompanies()
        {
            var loggedInUser = User.Identity!.Name!;
            ViewBag.Username = loggedInUser;
            //opens the booking companies page and calls for fleetService to get all companies
            var companies = _fleetService.GetAllCompanies();
            //forwards the companies
            return View("BookingCompanies", companies);
        }
    }
}