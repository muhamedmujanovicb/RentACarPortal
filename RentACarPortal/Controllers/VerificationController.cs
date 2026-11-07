using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Data;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class VerificationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokens;
        private readonly NotificationManager _notifications;

        public VerificationController(AppDbContext context, TokenService tokens, NotificationManager notifications)
        {
            _context = context;
            _tokens = tokens;
            _notifications = notifications;
        }

        [HttpGet]
        public IActionResult VerifyEmail(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                TempData["LoginError"] = "That verification link is invalid or has expired.";
                return RedirectToAction("LoginForm", "Login");
            }

            var hash = _tokens.Hash(token);
            var now = DateTime.UtcNow;

            var company = _context.Companies.FirstOrDefault(c =>
                c.VerificationTokenHash == hash && c.VerificationTokenExpiresAt > now);
            if (company != null)
            {
                company.IsEmailVerified = true;
                company.VerificationTokenHash = null;
                company.VerificationTokenExpiresAt = null;
                _context.SaveChanges();
                TempData["LoginInfo"] = "Your email is verified. You can now log in.";
                return RedirectToAction("LoginForm", "Login");
            }

            // repeat the same lookup for _context.Clients (you write this part)
            var client = _context.Clients.FirstOrDefault(cl =>
                cl.VerificationTokenHash == hash && cl.VerificationTokenExpiresAt > now);
            if (client != null)
            {
                client.IsEmailVerified = true;
                client.VerificationTokenHash = null;
                client.VerificationTokenExpiresAt = null;
                _context.SaveChanges();
                TempData["LoginInfo"] = "Your email is verified. You can now log in.";
                return RedirectToAction("LoginForm", "Login");
            }

            TempData["LoginError"] = "That verification link is invalid or has expired.";
            return RedirectToAction("LoginForm", "Login");
        }

        [HttpGet]
        public IActionResult ResendForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendVerification(string email)
        {
            email = (email ?? "").Trim().ToLowerInvariant();
            var now = DateTime.UtcNow;

            var token = _tokens.GenerateToken();
            var tokenHash = _tokens.Hash(token);
            var expiresAt = now.AddHours(24);
            var link = Url.Action("VerifyEmail", "Verification", new { token }, Request.Scheme)!;

            var company = _context.Companies.FirstOrDefault(c => c.Email == email && !c.IsEmailVerified);
            if (company != null && !RecentlySent(company.VerificationTokenExpiresAt, now))
            {
                company.VerificationTokenHash = tokenHash;
                company.VerificationTokenExpiresAt = expiresAt;
                _context.SaveChanges();
                await _notifications.SendVerificationEmailAsync(company.Email, company.Username, link);
            }

            var client = _context.Clients.FirstOrDefault(c => c.Email == email && !c.IsEmailVerified);
            if (client != null && !RecentlySent(client.VerificationTokenExpiresAt, now))
            {
                client.VerificationTokenHash = tokenHash;
                client.VerificationTokenExpiresAt = expiresAt;
                _context.SaveChanges();
                await _notifications.SendVerificationEmailAsync(client.Email, client.Username, link);
            }

            TempData["LoginInfo"] = "If an unverified account exists for that address, we've sent a new verification link.";
            return RedirectToAction("LoginForm", "Login");
        }

        private static bool RecentlySent(DateTime? expiresAt, DateTime now)
        {
            return expiresAt != null && expiresAt > now.AddHours(23).AddMinutes(55);
        }
    }
}