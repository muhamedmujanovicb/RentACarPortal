using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class FleetManagerController : Controller
    {
        private readonly FleetService _fleetService;

        public FleetManagerController(FleetService fleetService)
        {
            _fleetService = fleetService;
        }

        [HttpGet]
        public IActionResult FleetManager(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            ViewBag.Makes = _fleetService.GetAllMakes();
            ViewBag.Models = _fleetService.GetAllModels();
            ViewBag.VehicleTypes = _fleetService.GetAllVehicleTypes();
            return View("FleetManager");
        }

        [HttpPost]
        public IActionResult NewVehicle([Bind("Make,Model,Year,NumberOfSeats,IsDiesel,FuelEfficiency,FuelTankSize,TypeOfVehicle,HasChildrenSafety,DriveTerrain,BootSpace,ACtype,HasNavigation,DailyRate,HasInsurance,RegisterNumberOfVehicle,VehicleMakeId,VehicleModelId,VehicleTypeId")] Vehicle newVehicle, string loggedInUser)
        {
            _fleetService.AddVehicleToFleet(newVehicle, loggedInUser);
            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}