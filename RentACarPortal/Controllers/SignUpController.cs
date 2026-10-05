using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class SignUpController : Controller
    {
        private readonly AppDbContext _context;

        public SignUpController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SignUpForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessSignUp(string username, string password, bool isCompany)
        {
            if (isCompany)
            {
                // Create and save a Company account
                var newCompany = new Company(username, password);
                _context.Companies.Add(newCompany);
            }
            else
            {
                // Create and save a Client account
                var newClient = new Client(username, password);
                _context.Clients.Add(newClient);
            }

            _context.SaveChanges();

            return RedirectToAction("LoginForm", "Login");
        }
    }
}
