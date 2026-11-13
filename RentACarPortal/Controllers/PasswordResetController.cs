using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Data;
using RentACarPortal.Services;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class PasswordResetController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokens;
        private readonly NotificationManager _notifications;
        private readonly PasswordService _passwords;

        public PasswordResetController(AppDbContext context, TokenService tokens, NotificationManager notifications, PasswordService passwords)
        {
            _context = context;
            _tokens = tokens;
            _notifications = notifications;
            _passwords = passwords;
        }

        [HttpGet]
        public IActionResult ForgotPasswordForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            email = (email ?? "").Trim().ToLowerInvariant();
            var now = DateTime.UtcNow;

            var token = _tokens.GenerateToken();
            var tokenHash = _tokens.Hash(token);
            var expiresAt = now.AddHours(1);
            var link = Url.Action("ResetPasswordForm", "PasswordReset", new { token }, Request.Scheme)!;

            var company = _context.Companies.FirstOrDefault(c => c.Email == email && c.IsEmailVerified);
            if (company != null && !RecentlySent(company.ResetTokenExpiresAt, now))
            {
                company.ResetTokenHash = tokenHash;
                company.ResetTokenExpiresAt = expiresAt;
                _context.SaveChanges();
                await _notifications.SendPasswordResetEmailAsync(company.Email, company.Username, link);
            }

            var client = _context.Clients.FirstOrDefault(c => c.Email == email && c.IsEmailVerified);
            if (client != null && !RecentlySent(client.ResetTokenExpiresAt, now))
            {
                client.ResetTokenHash = tokenHash;
                client.ResetTokenExpiresAt = expiresAt;
                _context.SaveChanges();
                await _notifications.SendPasswordResetEmailAsync(client.Email, client.Username, link);
            }

            TempData["LoginInfo"] = "If an account exists for that address, we've sent a password reset link.";
            return RedirectToAction("LoginForm", "Login");
        }

        private static bool RecentlySent(DateTime? expiresAt, DateTime now)
        {
            return expiresAt != null && expiresAt > now.AddMinutes(55);
        }

        private const int MinPasswordLength = 8;
        private const int MaxPasswordLength = 128;

        private (Company? company, Client? client) FindByResetToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token)) return (null, null);

            var hash = _tokens.Hash(token);
            var now = DateTime.UtcNow;

            var company = _context.Companies.FirstOrDefault(c => c.ResetTokenHash == hash && c.ResetTokenExpiresAt > now);
            if (company != null) return (company, null);

            var client = _context.Clients.FirstOrDefault(c => c.ResetTokenHash == hash && c.ResetTokenExpiresAt > now);
            return (null, client);
        }

        [HttpGet]
        public IActionResult ResetPasswordForm(string token)
        {
            var (company, client) = FindByResetToken(token);
            if (company == null && client == null)
            {
                TempData["LoginError"] = "That reset link is invalid or has expired.";
                return RedirectToAction("LoginForm", "Login");
            }

            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string token, string newPassword, string confirmPassword)
        {
            var (company, client) = FindByResetToken(token);
            if (company == null && client == null)
            {
                TempData["LoginError"] = "That reset link is invalid or has expired.";
                return RedirectToAction("LoginForm", "Login");
            }

            newPassword ??= "";

            if (newPassword != confirmPassword)
            {
                ViewBag.ErrorMessage = "The passwords do not match.";
                ViewBag.Token = token;
                return View("ResetPasswordForm");
            }

            if (newPassword.Length < MinPasswordLength || newPassword.Length > MaxPasswordLength)
            {
                ViewBag.ErrorMessage = $"Password must be between {MinPasswordLength} and {MaxPasswordLength} characters.";
                ViewBag.Token = token;
                return View("ResetPasswordForm");
            }

            var hash = _passwords.Hash(newPassword);

            if (company != null)
            {
                company.Password = hash;
                company.ResetTokenHash = null;
                company.ResetTokenExpiresAt = null;
            }
            else if (client != null)
            {
                client.Password = hash;
                client.ResetTokenHash = null;
                client.ResetTokenExpiresAt = null;
            }

            _context.SaveChanges();

            TempData["LoginInfo"] = "Your password was changed. You can now log in.";
            return RedirectToAction("LoginForm", "Login");
        }
    }
}