using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;
using SQLitePCL;

namespace RentACarPortal.Controllers
{

    public class FleetOverviewController : Controller
    {

        private readonly AppDbContext _context;

        public FleetOverviewController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult FleetOverview()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteVehicle(int id, string loggedInUser)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }


}
