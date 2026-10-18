using Microsoft.AspNetCore.Identity;

namespace RentACarPortal.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string Hash(string password)
        {
            return _hasher.HashPassword(new object(), password);
        }

        public bool Verify(string storedHash, string providedPassword)
        {
            try
            {
                var result = _hasher.VerifyHashedPassword(new object(), storedHash, providedPassword);
                return result != PasswordVerificationResult.Failed;
            }
            catch (FormatException)
            {
                return false;
            }
           
        }
    }
}
