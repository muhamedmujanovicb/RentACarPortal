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

        //CONSTRUCTOR
        public Client() { }

        public Client(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
