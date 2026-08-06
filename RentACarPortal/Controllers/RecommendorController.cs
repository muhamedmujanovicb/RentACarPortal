using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class RecommendorController : Controller
    {
        private readonly AppDbContext _context;

        public RecommendorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allUsers = _context.Users.ToList();

            ViewBag.Companies = allUsers
                .Where(u => u.IsAdmin)
                .Select(u => u.Username)
                .ToList();

            return View("~/Views/UserDashboard/Recommendor.cshtml");
        }

        [HttpPost]
        public IActionResult GetRecommendation(VehicleRecommendation model)
        {
            var operationalFleet = _context.Vehicles.ToList();

            var survivingCars = operationalFleet
                .Where(car => car.NumberOfSeats >= model.passengerCount)
                .Where(car => !model.hasChildren || car.HasChildrenSafety)
                .Where(car =>
                {
                    double fuelPrice = model.fuelCost > 0 ? model.fuelCost : 1.5;
                    double totalCost = ((model.distance / 100) * car.FuelEfficiency) * fuelPrice + model.highwayCosts + (car.DailyRate * model.tripDuration);

                    return totalCost <= model.budget;
                })
                .ToList();

            foreach (var car in survivingCars)
            {
                car.Score = 0;

                if (model.weather == "blizzard" || model.driveTerrain == "offroad")
                {
                    if (car.DriveTerrain == "AWD" || car.DriveTerrain == "4WD") car.Score += 50;
                    if (car.DriveTerrain == "2WD") car.Score -= 50;
                }

                if (model.hasElderly)
                {
                    if (car.TypeOfVehicle == "SUV" || car.TypeOfVehicle == "Monovolumen") car.Score += 50;
                }

                if (model.distance > 500)
                {
                    if (car.ACtype == "Dual") car.Score += 20;
                    string[] comfortableTypes = { "Limo", "Sedan", "SUV", "Jeep", "Monovolumen" };
                    if (comfortableTypes.Contains(car.TypeOfVehicle)) car.Score += 20;
                }

                if (model.hasGps && car.HasNavigation)
                {
                    car.Score += 10;
                }
            }

            var finalRecommendations = survivingCars.OrderByDescending(c => c.Score).ToList();

            return View("~/Views/UserDashboard/Recommendations.cshtml", finalRecommendations);
        }
    }
}
