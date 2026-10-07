namespace RentACarPortal.Models
{
    public class Contract
    {
        //IMPORTANT ATTRIBUTES
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
        public string Status { get; set; } = "Active";
        public int? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }


        //OTHER ATTRIBUTES
        public string DriverFullName { get; set; } = string.Empty;
        public string RentalStation { get; set; } = string.Empty;
        public string TypeOfVehicle { get; set; } = string.Empty;
        public string RegisterNumberOfVehicle { get; set; } = string.Empty;
        public string RentDriver { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string PlaceOfIssue { get; set; } = string.Empty;
        public DateOnly DateOfIssue { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PersonalNumber { get; set; } = string.Empty;
        public string DrivingLicenseNumber { get; set; } = string.Empty;
        public DateOnly RentalStartDate { get; set; }
        public TimeOnly RentalStartTime { get; set; }
        public string RentalStartPlace { get; set; } = string.Empty;
        public DateOnly RentalEndDate { get; set; }
        public TimeOnly RentalEndTime { get; set; }
        public string RentalEndPlace { get; set; } = string.Empty;
        public bool Insurance { get; set; }
        public string FuelRecieved { get; set; } = string.Empty;
        public string FuelReturned { get; set; } = string.Empty;
        public double FullTankSizeLiquid { get; set; }
        public double Deposit { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }
}
