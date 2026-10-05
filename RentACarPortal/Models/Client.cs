namespace RentACarPortal.Models
{
    public class Client
    {
        //IMPORTANT ATTRIBUTES
        public int Id { get; set; }
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        //OTHER ATTRIBUTES
        public string Username { get; set; }
        public string Password { get; set; }

        //CONSTRUCTOR
        public Client() { }

        public Client(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
