using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RentACarPortal.Controllers
{
    public class BookingContractRequestController : Controller
    {
        [HttpGet]
        public IActionResult BookingContractRequest(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;

            return View("~/Views/UserDashboard/BookingContractRequest.cshtml");
        }
    }
}
