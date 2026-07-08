using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult LoginForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessLogin(string username, string password)
        {
            if (username == "admin" && password == "123")
            {
                return RedirectToAction("Dashboard", "Fleet", new { loggedInUser = "Admin" });
            }

            var matchingUser = SignUpController.MockUserDatabase
        .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (matchingUser != null)
            {
                if (matchingUser.IsAdmin)
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
