using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;
using System.Collections.Generic;

namespace RentACarPortal.Controllers
{
    public class FleetController : Controller
    {
        public IActionResult Index()
        {
            var operationalFleet = new List<Vehicle>
            {
                new Vehicle
                {
                    Id = 1,
                    Make = "Audi",
                    Model = "A6",
                    Year = 2022,
                    DailyRate = 90.00,
                    HasInsurance = true,
                    Status = "Available",
                    RegisterNumberOfVehicle = "TX-789-AA"
                },
                new Vehicle
                {
                    Id = 2,
                    Make = "Volkswagen",
                    Model = "Golf 8",
                    Year = 2023,
                    DailyRate = 65.00,
                    HasInsurance = true,
                    Status = "Rented",
                    RegisterNumberOfVehicle = "SA-123-BB"
                },
                new Vehicle
                {
                    Id = 3,
                    Make = "BMW",
                    Model = "530d",
                    Year = 2021,
                    DailyRate = 110.00,
                    HasInsurance = false,
                    Status = "Maintenance",
                    RegisterNumberOfVehicle = "TZ-456-CC"
                }
            };

            return View(operationalFleet);
        }
    }
}