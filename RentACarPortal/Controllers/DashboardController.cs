using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class DashboardController : Controller
    {
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
    }
}
