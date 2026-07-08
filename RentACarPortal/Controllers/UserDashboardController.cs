using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class UserDashboardController : Controller
    {
        [HttpGet]
        public IActionResult UserDashboard(string loggedInUser = "Admin")
        {
            ViewBag.Username = loggedInUser;

            return View();
        }
    }
}
