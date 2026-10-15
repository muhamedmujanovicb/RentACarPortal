using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class ContractHistoryController : Controller
    {
        private readonly DashboardService _dashboardService;

        public ContractHistoryController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult ContractHistory(string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            var contracts = _dashboardService.GetContractHistory();
            return View(contracts);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus, string loggedInUser)
        {
            _dashboardService.UpdateContractStatus(id, newStatus);
            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}