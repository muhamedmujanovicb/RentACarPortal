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
            var matchingUser = _context.Users.FirstOrDefault(u=> u.Username == username && u.Password == password);

            if (matchingUser != null)
            {
                if(matchingUser.IsAdmin)
                {
                    return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = matchingUser.Username });
                }
                else
                {
                    return RedirectToAction("UserDashboard", "UserDashboard", new { loggedInUser = matchingUser.Username });
                }
            }

            ViewBag.ErrorMessage = "Invalid username or password";
            return View("LoginForm");
        }
    }
}
