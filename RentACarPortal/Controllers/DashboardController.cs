using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Models;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardService _dashboardService;
        private readonly FleetService _fleetService;

        public DashboardController(DashboardService dashboardService, FleetService fleetService)
        {
            _dashboardService = dashboardService;
            _fleetService = fleetService;
        }

        [HttpGet]
        public IActionResult Dashboard(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            var validContracts = _dashboardService.GetPendingContractsAndProcessExpirations(loggedInUser);
            return View(validContracts);
        }

        [HttpGet]
        public IActionResult FleetManager(string loggedInUser)
        {
            ViewBag.Username = string.IsNullOrEmpty(loggedInUser) ? "Admin" : loggedInUser;
            ViewBag.Makes = _fleetService.GetAllMakes();
            ViewBag.Models = _fleetService.GetAllModels();
            ViewBag.VehicleTypes = _fleetService.GetAllVehicleTypes();
            return View("FleetManager");
        }

        [HttpGet]
        public IActionResult FleetOverview(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            var vehicles = _dashboardService.GetCompanyFleetWithLookups(loggedInUser);
            return View(vehicles);
        }

        [HttpGet]
        public IActionResult ContractCreator(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            return View("ContractCreator");
        }

        [HttpGet]
        public IActionResult ContractHistory(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            var contracts = _dashboardService.GetCompanyContracts(loggedInUser);
            return View(contracts);
        }

        [HttpGet]
        public IActionResult ContractRequestDetails(int id, string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            var (contract, vehicle) = _dashboardService.GetContractRequestDetails(id);

            if (contract == null)
            {
                return NotFound();
            }

            ViewBag.Vehicle = vehicle;
            return View("ContractRequestDetails", contract);
        }

        [HttpGet]
        public IActionResult Statistics(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            return RedirectToAction("Index", "Statistics", new { loggedInUser });
        }
    }
}
