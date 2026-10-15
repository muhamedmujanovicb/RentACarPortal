using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class ContractCreatorController : Controller
    {
        private readonly DashboardService _dashboardService;

        public ContractCreatorController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult ContractCreator(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            return View();
        }

        [HttpPost]
        public IActionResult NewContract([Bind("RentalStation,TypeOfVehicle,RegisterNumberOfVehicle,RentDriver,Address,Telephone,PassportNumber,PlaceOfIssue,DateOfIssue,DateOfBirth,PersonalNumber,DrivingLicenseNumber,RentalStartDate,RentalStartTime,RentalStartPlace,RentalEndDate,RentalEndTime,RentalEndPlace,Insurance,FuelRecieved,FuelReturned,FullTankSizeLiquid,Deposit,Comment,Remarks")] Contract newContract, string loggedInUser)
        {
            _dashboardService.CreateContract(newContract, loggedInUser);
            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}