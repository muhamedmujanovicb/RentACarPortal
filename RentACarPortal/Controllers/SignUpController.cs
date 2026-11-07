using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Data;
using RentACarPortal.Models;
using RentACarPortal.Services;
using System.Net.Mail;

namespace RentACarPortal.Controllers
{
    public class SignUpController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwords;
        private readonly TokenService _tokens;
        private readonly NotificationManager _notifications;

        public SignUpController(AppDbContext context, PasswordService passwords,
                TokenService tokens, NotificationManager notifications)
        {
            _context = context;
            _passwords = passwords;
            _tokens = tokens;
            _notifications = notifications;
        }

        [HttpGet]
        public IActionResult SignUpForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessSignUp(string username, string email, string password, bool isCompany)
        {
            /*
             * tries to parse the text as an email and returns false if it fails
             * checks if the cleaned address differs from the typed in one
             * checks if the typed in email is longer than the standard length for an email which is 254
             */
            if (!MailAddress.TryCreate(email, out var parsed) || parsed.Address != email || email.Length > 254)
            {
                ViewBag.ErrorMessage = "Please enter a valid email address.";
                return View("SignUpForm");
            }

            //cleans up the email, protects from null if someone sends a request without the field, trims the email in case it has empty spaces and puts it to lowercase
            email = (email ?? "").Trim().ToLowerInvariant();

            //queries to check if any companies or clients exist with the same userrname
            bool usernameTaken = _context.Companies.Any(c => c.Username == username) || _context.Clients.Any(cl => cl.Username == username);

            //if the username is taken then it throws an error
            if (usernameTaken)
            {
                ViewBag.ErrorMessage = "That username is already taken.";
                return View("SignUpForm");
            }
            //duplicate check for email
            bool emailTaken = _context.Companies.Any(c => c.Email == email) || _context.Clients.Any(cl => cl.Email == email);

            if (emailTaken)
            {
                ViewBag.ErrorMessage = "That email is already registered.";
                return View("SignUpForm");
            }

            //calls from PasswordService the hash method
            var hash = _passwords.Hash(password);

            //generates the token, hashes it and adds an expiration time stamp
            var token = _tokens.GenerateToken();
            var tokenHash = _tokens.Hash(token);
            var expiresAt = DateTime.UtcNow.AddHours(24);

            if (isCompany)
            {
                _context.Companies.Add(new Company(username, email, hash)
                {
                    VerificationTokenHash = tokenHash,
                    VerificationTokenExpiresAt = expiresAt
                });
            }
            else
            {
                _context.Clients.Add(new Client(username, email, hash)
                {
                    VerificationTokenHash = tokenHash,
                    VerificationTokenExpiresAt = expiresAt
                });
            }

            _context.SaveChanges();

            var link = Url.Action("VerifyEmail", "Verification", new { token }, Request.Scheme);
            await _notifications.SendVerificationEmailAsync(email, username, link!);

            TempData["LoginInfo"] = "Account created. Check your email and click the link to verify before logging in.";
            return RedirectToAction("LoginForm", "Login");
        }
    }
}
