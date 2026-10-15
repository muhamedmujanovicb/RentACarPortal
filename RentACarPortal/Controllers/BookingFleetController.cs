using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class BookingFleetController : Controller
    {
        private readonly FleetService _fleetService;

        public BookingFleetController(FleetService fleetService)
        {
            _fleetService = fleetService;
        }

        [HttpGet]
        public async Task<IActionResult> BookingFleet(string loggedInUser, string companyName)
        {
            ViewBag.Username = loggedInUser;
            ViewBag.SelectedCompany = companyName;

            var availableVehicles = await _fleetService.GetAvailableVehiclesByCompanyAsync(companyName);

            return View("~/Views/UserDashboard/BookingFleet.cshtml", availableVehicles);
        }
    }
}