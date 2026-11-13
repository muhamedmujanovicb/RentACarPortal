namespace RentACarPortal.Models
{
    public class Client
    {
        //IMPORTANT ATTRIBUTES
        public int Id { get; set; }
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        //OTHER ATTRIBUTES
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public string? VerificationTokenHash { get; set; }
        public DateTime? VerificationTokenExpiresAt { get; set; }
        public string? ResetTokenHash { get; set; }
        public DateTime? ResetTokenExpiresAt { get; set; }

        //CONSTRUCTOR
        public Client() { }

        public Client(string username, string email ,string password)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
