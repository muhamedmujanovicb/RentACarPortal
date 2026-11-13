namespace RentACarPortal.Services
{
    public record ContractEmailData(
        string ClientEmail,
        string ClientUsername,
        string CompanyEmail,
        string CompanyName,      // the company's Username, since Company has no display name
        string VehicleName,      // from Contract.TypeOfVehicle
        string RegisterNumber,
        DateOnly StartDate,
        DateOnly EndDate,
        double TotalPrice,
        string DriverName,       // Contract.RentDriver, because DriverFullName is never filled
        string Comment,          // the client's note
        string Remarks           // the company's reply, empty for the "request sent" emails
    );
}
