using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;
using RentACarPortal.Data;
using System.Linq;

namespace RentACarPortal.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult LoginForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessLogin(string username, string password)
        {
            // Check if it's a Company account 
            var matchingCompany = _context.Companies
                .FirstOrDefault(c => c.Username == username && c.Password == password);

            if (matchingCompany != null)
            {
                return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = matchingCompany.Username });
            }

            // Check if it's a Client account 
            var matchingClient = _context.Clients
                .FirstOrDefault(cl => cl.Username == username && cl.Password == password);

            if (matchingClient != null)
            {
                return RedirectToAction("UserDashboard", "UserDashboard", new { loggedInUser = matchingClient.Username });
            }

            // If neither matches, show error
            ViewBag.ErrorMessage = "Invalid username or password";
            return View("LoginForm");
        }
    }
}
