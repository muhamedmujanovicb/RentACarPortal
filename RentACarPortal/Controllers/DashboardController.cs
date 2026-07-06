using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard(string loggedInUser = "Admin")
        {
            ViewBag.Username = loggedInUser;

            return View();
        }
    }
}
