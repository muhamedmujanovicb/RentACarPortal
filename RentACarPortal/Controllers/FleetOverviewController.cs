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
    }


}
