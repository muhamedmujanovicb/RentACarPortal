using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Data;
using RentACarPortal.Models;
using SQLitePCL;

namespace RentACarPortal.Controllers
{
    public class FleetManagerController : Controller
    {
        private readonly AppDbContext _context;

        public FleetManagerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult FleetManager()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewVehicle([Bind("Make,Model,Year,NumberOfSeats,IsDiesel,FuelEfficiency,FuelTankSize,TypeOfVehicle,HasChildrenSafety,DriveTerrain,BootSpace,ACtype,HasNavigation,DailyRate,HasInsurance,RegisterNumberOfVehicle")] Vehicle newVehicle, string loggedInUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == loggedInUser);
            if (user != null)
            {
                newVehicle.UserId = user.Id;
                _context.Vehicles.Add(newVehicle);
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}
