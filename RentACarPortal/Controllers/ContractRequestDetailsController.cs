using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Controllers
{
    public class ContractRequestDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public ContractRequestDetailsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ContractRequestDetails(int id, string loggedInUser)
        {
            ViewBag.Username = loggedInUser;

            var contract = _context.Contracts
                .Include(c => c.Vehicle)
                .Include(c => c.Client)
                .FirstOrDefault(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        [HttpPost]
        public IActionResult AcceptRequest(int id, string loggedInUser)
        {
            var contract = _context.Contracts
        .Include(c => c.Vehicle)
        .FirstOrDefault(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            var companyUser = _context.Companies.FirstOrDefault(c => c.Username == loggedInUser);
            if (companyUser == null)
            {
                return Unauthorized();
            }

            contract.Status = "Active";
            contract.RentalStation = loggedInUser;
            contract.Remarks = "Created from approved contract request.";

            contract.CompanyId = companyUser.Id;

            if (contract.Vehicle != null)
            {
                contract.Vehicle.Status = "Rented";
                contract.Vehicle.HoldExpiresAt = null;
            }

            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }

        [HttpPost]
        public IActionResult DeclineRequest(int id, string loggedInUser)
        {
            var contract = _context.Contracts
                .Include(c => c.Vehicle)
                .FirstOrDefault(c => c.Id == id);

            if (contract == null)
            {
                return NotFound();
            }

            var companyUser = _context.Companies.FirstOrDefault(c => c.Username == loggedInUser);
            if (companyUser == null)
            {
                return Unauthorized();
            }

            if (contract.Vehicle != null)
            {
                contract.Vehicle.Status = "Available";
                contract.Vehicle.HoldExpiresAt = null;
            }

            _context.Contracts.Remove(contract);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}
