using System.Net;

namespace RentACarPortal.Services
{
    public class NotificationManager
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<NotificationManager> _logger;

        public NotificationManager(IEmailSender emailSender, ILogger<NotificationManager> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        private async Task TrySendAsync(string to, string subject, string body)
        {
            try
            {
                await _emailSender.SendAsync(to, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email '{Subject}' to {To}", subject, to);
            }
        }

        public Task SendVerificationEmailAsync(string to, string username, string verificationLink)
        {
            var safeName = WebUtility.HtmlEncode(username);
            var body = $@"<p>Hello {safeName},</p>
            <p>Welcome to FleetDrive. Please confirm your email address by clicking the link below:</p>
            <p><a href=""{verificationLink}"">Verify my email</a></p>
            <p>This link expires in 24 hours. If you didn't create this account, you can ignore this email.</p>";

            return TrySendAsync(to, "Verify your FleetDrive email", body);
        }
    }
}
