using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;
using RentACarPortal.Data;
using System.Linq;
using RentACarPortal.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace RentACarPortal.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwords;

        public LoginController(AppDbContext context, PasswordService passwords)
        {
            _context = context;
            _passwords = passwords;
        }

        [HttpGet]
        public IActionResult LoginForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessLogin(string username, string password)
        {
            var loggedInUser = User.Identity!.Name!;
            var matchingCompany = _context.Companies.FirstOrDefault(c => c.Username == username);
            if (matchingCompany != null && _passwords.Verify(matchingCompany.Password, password))
            {
                await SignInUser(matchingCompany.Id, matchingCompany.Username, "Company");
                return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = matchingCompany.Username });
            }

            var matchingClient = _context.Clients.FirstOrDefault(cl => cl.Username == username);
            if (matchingClient != null && _passwords.Verify(matchingClient.Password, password))
            {
                await SignInUser(matchingClient.Id, matchingClient.Username, "Client");
                return RedirectToAction("UserDashboard", "UserDashboard", new { loggedInUser = matchingClient.Username });
            }

            ViewBag.ErrorMessage = "Invalid username or password";
            return View("LoginForm");
        }

        private async Task SignInUser(int id, string username, string role)
        {
            var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, id.ToString()),
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Role, role)
      };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginForm");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
