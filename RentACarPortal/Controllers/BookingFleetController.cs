using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACarPortal.Controllers
{
    [Authorize(Roles = "Client")]

    public class BookingFleetController : Controller
    {
        private readonly FleetService _fleetService;

        public BookingFleetController(FleetService fleetService)
        {
            _fleetService = fleetService;
        }

        [HttpGet]
        public async Task<IActionResult> BookingFleet(string companyName)
        {
            var loggedInUser = User.Identity!.Name!;
            ViewBag.Username = loggedInUser;
            ViewBag.SelectedCompany = companyName;

            var availableVehicles = await _fleetService.GetAvailableVehiclesByCompanyAsync(companyName);

            return View(availableVehicles);
        }
    }
}