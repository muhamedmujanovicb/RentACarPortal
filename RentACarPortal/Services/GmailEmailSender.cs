using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace RentACarPortal.Services  
{
    public class GmailEmailSender : IEmailSender
    {
        private readonly string _address;
        private readonly string _appPassword;

        public GmailEmailSender(IConfiguration configuration)
        {
            _address = configuration["Email:Address"]
                ?? throw new InvalidOperationException("Email:Address is not configured.");
            _appPassword = configuration["Email:AppPassword"]
                ?? throw new InvalidOperationException("Email:AppPassword is not configured.");
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            //message build
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RentACarPortal", _address));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            //sending the message
            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_address, _appPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
