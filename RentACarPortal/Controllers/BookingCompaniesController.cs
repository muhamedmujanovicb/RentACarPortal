using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class BookingCompaniesController : Controller
    {
        private readonly AppDbContext _context;

        public BookingCompaniesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult BookingCompanies(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;

            var companies = _context.Users.Where(u => u.IsAdmin).ToList();

            return View(companies);
        }


    }
}
