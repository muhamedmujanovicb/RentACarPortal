using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class SignUpController : Controller
    {
        [HttpGet]
        public IActionResult SignUpForm()
        {
            return View();
        }

        public static List<RentACarPortal.Models.User> MockUserDatabase = new List<RentACarPortal.Models.User>();

        [HttpPost]
        public IActionResult ProcessSignUp(string username, string password, bool isAdmin)
        {
            var newUser = new RentACarPortal.Models.User(username, password, isAdmin);
            MockUserDatabase.Add(newUser);

            return RedirectToAction("LoginForm", "Login");
        }
    }
}
