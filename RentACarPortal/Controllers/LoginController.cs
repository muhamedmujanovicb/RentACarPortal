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
            var matchingUser = SignUpController.MockUserDatabase
        .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (matchingUser != null || (username == "admin" && password == "123"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid username or password";
            return View("LoginForm");
        }
    }
}
