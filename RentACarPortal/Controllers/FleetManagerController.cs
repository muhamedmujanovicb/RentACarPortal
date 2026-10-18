using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;
using RentACarPortal.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACarPortal.Controllers
{
    [Authorize(Roles = "Company")]

    public class FleetManagerController : Controller
    {
        private readonly FleetService _fleetService;

        public FleetManagerController(FleetService fleetService)
        {
            _fleetService = fleetService;
        }

        [HttpGet]
        public IActionResult FleetManager()
        {
            ViewBag.Username = User.Identity!.Name!;
            ViewBag.Makes = _fleetService.GetAllMakes();
            ViewBag.Models = _fleetService.GetAllModels();
            ViewBag.VehicleTypes = _fleetService.GetAllVehicleTypes();
            return View("FleetManager");
        }

        [HttpPost]
        public IActionResult NewVehicle([Bind("Make,Model,Year,NumberOfSeats,IsDiesel,FuelEfficiency,FuelTankSize,TypeOfVehicle,HasChildrenSafety,DriveTerrain,BootSpace,ACtype,HasNavigation,DailyRate,HasInsurance,RegisterNumberOfVehicle,VehicleMakeId,VehicleModelId,VehicleTypeId")] Vehicle newVehicle)
        {
            var loggedInUser = User.Identity!.Name!;
            var error = _fleetService.AddVehicleToFleet(newVehicle, loggedInUser);
            if (error != null)
            {
                TempData["FleetError"] = error;
                return RedirectToAction("FleetManager", "FleetManager");
            }
            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
}