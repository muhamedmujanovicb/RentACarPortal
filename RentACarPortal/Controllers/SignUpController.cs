using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Data;
using RentACarPortal.Models;
using RentACarPortal.Services;
using System.Reflection.Metadata;

namespace RentACarPortal.Controllers
{
    public class SignUpController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwords;

        public SignUpController(AppDbContext context, PasswordService passwords)
        {
            _context = context;
            _passwords = passwords;
        }

        [HttpGet]
        public IActionResult SignUpForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessSignUp(string username, string password, bool isCompany)
        {
            bool usernameTaken = _context.Companies.Any(c => c.Username == username) || _context.Clients.Any(cl => cl.Username == username);

            if (usernameTaken)
            {
                ViewBag.ErrorMessage = "That username is already taken.";
                return View("SignUpForm");
            }


            var hash = _passwords.Hash(password);

            if (isCompany)
            {
                // Create and save a Company account
                _context.Companies.Add(new Company(username, hash));
            }
            else
            {
                // Create and save a Client account
                _context.Clients.Add(new Client(username, hash));
            }

            _context.SaveChanges();

            return RedirectToAction("LoginForm", "Login");
        }
    }
}
