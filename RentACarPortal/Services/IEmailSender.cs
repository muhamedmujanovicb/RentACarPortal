using System.Threading.Tasks;

namespace RentACarPortal.Services
{
        public interface IEmailSender
        {
        Task SendAsync(string to, string subject, string body);
        }
}
