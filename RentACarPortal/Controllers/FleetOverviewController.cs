using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class FleetOverviewController : Controller
    {
        private readonly DashboardService _dashboardService;
        private readonly FleetService _fleetService;

        public FleetOverviewController(DashboardService dashboardService, FleetService fleetService)
        {
            _dashboardService = dashboardService;
            _fleetService = fleetService;
        }

        public IActionResult FleetOverview(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;

            var vehicles = _dashboardService.GetCompanyFleetWithLookups(loggedInUser);

            return View(vehicles);
        }

        [HttpPost]
        public IActionResult DeleteVehicle(int id, string loggedInUser)
        {
            _fleetService.DeleteVehicle(id);

            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}