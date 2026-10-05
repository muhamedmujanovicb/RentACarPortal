namespace RentACarPortal.Models
{
    public class Company
    {
        //IMPORTANT ATTRIBUTES
        public int Id { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        //OTHER ATTRIBUTES
        public string Username { get; set; }
        public string Password { get; set; }

        //CONSTRUCTOR
        public Company() { }

        public Company(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
