using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Models;
using RentACarPortal.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACarPortal.Controllers
{
    [Authorize(Roles = "Company")]

    public class ContractCreatorController : Controller
    {
        private readonly DashboardService _dashboardService;

        public ContractCreatorController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult ContractCreator()
        {
            var loggedInUser = User.Identity!.Name!;
            ViewBag.Username = loggedInUser;
            return View("ContractCreator");
        }

        [HttpPost]
        public IActionResult NewContract([Bind("RentalStation,TypeOfVehicle,RegisterNumberOfVehicle,RentDriver,Address,Telephone,PassportNumber,PlaceOfIssue,DateOfIssue,DateOfBirth,PersonalNumber,DrivingLicenseNumber,RentalStartDate,RentalStartTime,RentalStartPlace,RentalEndDate,RentalEndTime,RentalEndPlace,Insurance,FuelRecieved,FuelReturned,FullTankSizeLiquid,Deposit,Comment,Remarks")] Contract newContract)
        {
            var loggedInUser = User.Identity!.Name!;
            _dashboardService.CreateContract(newContract, loggedInUser);
            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}