namespace RentACarPortal.Models
{
    public class Company
    {
        //IMPORTANT ATTRIBUTES
        public int Id { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        //OTHER ATTRIBUTES
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        //CONSTRUCTOR
        public Company() { }

        public Company(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
