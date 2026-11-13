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

        public Task SendRequestSentToClientAsync(ContractEmailData d)
        {
            var body = $@"<p>Hello {WebUtility.HtmlEncode(d.ClientUsername)},</p>
                <p>Your rental request has been sent to {WebUtility.HtmlEncode(d.CompanyName)}.</p>
                <p>Vehicle: {WebUtility.HtmlEncode(d.VehicleName)} ({WebUtility.HtmlEncode(d.RegisterNumber)})<br/>
                From: {d.StartDate:yyyy-MM-dd} To: {d.EndDate:yyyy-MM-dd}<br/>
                Total price: {d.TotalPrice:0.00}</p>
                <p>You will get another email when the company answers.</p>";

            return TrySendAsync(d.ClientEmail, "Your FleetDrive rental request was sent", body);
        }

        public Task SendNewRequestToCompanyAsync(ContractEmailData d)
        {
            if (string.IsNullOrWhiteSpace(d.CompanyEmail)) return Task.CompletedTask;

            var note = string.IsNullOrWhiteSpace(d.Comment) ? "No note" : WebUtility.HtmlEncode(d.Comment);

            var body = $@"<p>Hello {WebUtility.HtmlEncode(d.CompanyName)},</p>
                <p>You have a new rental request from {WebUtility.HtmlEncode(d.ClientUsername)}.</p>
                <p>Driver: {WebUtility.HtmlEncode(d.DriverName)}<br/>
                Vehicle: {WebUtility.HtmlEncode(d.VehicleName)} ({WebUtility.HtmlEncode(d.RegisterNumber)})<br/>
                From: {d.StartDate:yyyy-MM-dd} To: {d.EndDate:yyyy-MM-dd}<br/>
                Total price: {d.TotalPrice:0.00}</p>
                <p>Client's note: {note}</p>
                <p>Log in to your dashboard to accept or decline.</p>";

            return TrySendAsync(d.CompanyEmail, "New rental request on FleetDrive", body);
        }

        public Task SendRequestDeclinedAsync(ContractEmailData d)
        {
            var reply = string.IsNullOrWhiteSpace(d.Remarks) ? "No reason given" : WebUtility.HtmlEncode(d.Remarks);

            var body = $@"<p>Hello {WebUtility.HtmlEncode(d.ClientUsername)},</p>
                <p>Unfortunately {WebUtility.HtmlEncode(d.CompanyName)} declined your rental request.</p>
                <p>Vehicle: {WebUtility.HtmlEncode(d.VehicleName)} ({WebUtility.HtmlEncode(d.RegisterNumber)})<br/>
                From: {d.StartDate:yyyy-MM-dd} To: {d.EndDate:yyyy-MM-dd}</p>
                <p>Company's reply: {reply}</p>";

            return TrySendAsync(d.ClientEmail, "Your FleetDrive rental request was declined", body);
        }

        public Task SendRequestAcceptedAsync(ContractEmailData d)
        {
            var reply = string.IsNullOrWhiteSpace(d.Remarks) ? "No message" : WebUtility.HtmlEncode(d.Remarks);

            var body = $@"<p>Hello {WebUtility.HtmlEncode(d.ClientUsername)},</p>
                <p>Good news: {WebUtility.HtmlEncode(d.CompanyName)} accepted your rental request.</p>
                <p>Vehicle: {WebUtility.HtmlEncode(d.VehicleName)} ({WebUtility.HtmlEncode(d.RegisterNumber)})<br/>
                From: {d.StartDate:yyyy-MM-dd} To: {d.EndDate:yyyy-MM-dd}<br/>
                Total price: {d.TotalPrice:0.00}</p>
                <p>Company's reply: {reply}</p>";

            return TrySendAsync(d.ClientEmail, "Your FleetDrive rental request was accepted", body);
        }

        public Task SendPasswordResetEmailAsync(string to, string username, string resetLink)
        {
            var safeName = WebUtility.HtmlEncode(username);
            var body = $@"<p>Hello {safeName},</p>
                    <p>We received a request to reset your FleetDrive password. Click the link below to choose a new one:</p>
                    <p><a href=""{resetLink}"">Reset my password</a></p>
                    <p>This link expires in 1 hour and can be used once. If you didn't request this, you can ignore this email and your password will stay the same.</p>";

            return TrySendAsync(to, "Reset your FleetDrive password", body);
        }
    }
}
