using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Data;
using RentACarPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace RentACarPortal.Controllers
{
    public class ContractHistoryController : Controller
    {
        private readonly AppDbContext _context;

        public ContractHistoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ContractHistory(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;

            var contracts = _context.Contracts
                .Include(c => c.User)
                .ToList();

            return View();
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus, string loggedInUser)
        {
            var contract = _context.Contracts.Find(id);
            if (contract != null)
            {
                contract.Status = newStatus;
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}
