using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class ContractCreatorController : Controller
    {
        private readonly AppDbContext _context;

        public ContractCreatorController(AppDbContext context)
        {
            _context = context;
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
            var user = _context.Users.FirstOrDefault(u => u.Username == loggedInUser);
            if (user != null)
            {
                newContract.UserId = user.Id;
                _context.Contracts.Add(newContract);
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}
