using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class BookingCompaniesController : Controller
    {
        private readonly FleetService _fleetService;

        public BookingCompaniesController(FleetService fleetService)
        {
            _fleetService = fleetService;
        }

        [HttpGet]
        public IActionResult BookingCompanies(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            var companies = _fleetService.GetAllCompanies();
            return View(companies);
        }
    }
}