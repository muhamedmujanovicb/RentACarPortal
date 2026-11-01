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
            //calls fleetservice to get all vehicle makes
            ViewBag.Makes = _fleetService.GetAllMakes();
            //calls fleetservice to get all vehicle models
            ViewBag.Models = _fleetService.GetAllModels();
            //calls fleetservice to get all vehicle types
            ViewBag.VehicleTypes = _fleetService.GetAllVehicleTypes();
            return View("FleetManager");
        }

        [HttpPost]
        public IActionResult NewVehicle([Bind("Make,Model,Year,NumberOfSeats,IsDiesel,FuelEfficiency,FuelTankSize,TypeOfVehicle,HasChildrenSafety,DriveTerrain,BootSpace,ACtype,HasNavigation,DailyRate,HasInsurance,RegisterNumberOfVehicle,VehicleMakeId,VehicleModelId,VehicleTypeId")] Vehicle newVehicle)
        {
            var loggedInUser = User.Identity!.Name!;
            //checks for errors when calling fleetservice to add a vehicle to the company fleet
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