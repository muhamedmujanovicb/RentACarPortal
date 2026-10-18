using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACarPortal.Controllers
{
    [Authorize(Roles = "Company")]

    public class FleetOverviewController : Controller
    {
        private readonly DashboardService _dashboardService;
        private readonly FleetService _fleetService;

        public FleetOverviewController(DashboardService dashboardService, FleetService fleetService)
        {
            _dashboardService = dashboardService;
            _fleetService = fleetService;
        }

        [HttpGet]
        public IActionResult FleetOverview()
        {
            var loggedInUser = User.Identity!.Name!;
            ViewBag.Username = loggedInUser;
            var vehicles = _dashboardService.GetCompanyFleetWithLookups(loggedInUser);
            return View(vehicles);
        }

        [HttpPost]
        public IActionResult DeleteVehicle(int id)
        {
            var loggedInUser = User.Identity!.Name!;
            if (!_fleetService.DeleteVehicle(id, loggedInUser)) return NotFound();
            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
}