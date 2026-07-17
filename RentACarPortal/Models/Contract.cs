namespace RentACarPortal.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Status { get; set; } = "Active";

        public string RentalStation { get; set; }
        public string TypeOfVehicle { get; set; }
        public string RegisterNumberOfVehicle { get; set; }
        public string RentDriver { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string PassportNumber { get; set; }
        public string PlaceOfIssue { get; set; }
        public DateOnly DateOfIssue { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int PersonalNumber { get; set; }
        public int DrivingLicenseNumber { get; set; }
        public DateOnly RentalStartDate { get; set; }
        public TimeOnly RentalStartTime { get; set; }
        public string RentalStartPlace { get; set; }
        public DateOnly RentalEndDate { get; set; }
        public TimeOnly RentalEndTime { get; set; }
        public string RentalEndPlace { get; set; }
        public bool Insurance { get; set; }
        public string FuelRecieved { get; set; }
        public string FuelReturned { get; set; }
        public double FullTankSizeLiquid { get; set; }
        public double Deposit { get; set; }
        public string Comment { get; set; }
        public string Remarks { get; set; }
    }
}
