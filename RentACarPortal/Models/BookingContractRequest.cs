namespace RentACarPortal.Models
{
    public class BookingContractRequest
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Username { get; set; }
        public string CompanyId { get; set; }
        public Vehicle Vehicle { get; set; }

        public string DriverFullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PersonalIdNumber { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public string PassportNumber { get; set; }
        public string PlaceOfIssue { get; set; }
        public DateOnly DateOfIssue { get; set; }
        public string Notes { get; set; }
    }
}
