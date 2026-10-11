using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{

    public class FleetOverviewController : Controller
    {

        private readonly AppDbContext _context;

        public FleetOverviewController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult FleetOverview(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;

            // Fetch the user along with their associated fleet vehicles
            var user = _context.Companies
                .Include(u => u.Vehicles)
                .FirstOrDefault(u => u.Username == loggedInUser);

            var vehicles = user?.Vehicles.ToList() ?? new List<Vehicle>();

            return View(vehicles);
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
