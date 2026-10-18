using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACarPortal.Controllers
{
    [Authorize(Roles = "Company")]

    public class ContractHistoryController : Controller
    {
        private readonly DashboardService _dashboardService;

        public ContractHistoryController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult ContractHistory()
        {
            var loggedInUser = User.Identity!.Name!;
            ViewBag.Username = loggedInUser;
            var contracts = _dashboardService.GetCompanyContracts(loggedInUser);
            return View(contracts);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            var loggedInUser = User.Identity!.Name!;
            if (!_dashboardService.UpdateContractStatus(id, newStatus, loggedInUser)) return NotFound();
            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
}