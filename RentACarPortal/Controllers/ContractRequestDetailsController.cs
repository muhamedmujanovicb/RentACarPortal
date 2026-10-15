using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class ContractRequestDetailsController : Controller
    {
        private readonly DashboardService _dashboardService;

        public ContractRequestDetailsController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult ContractRequestDetails(int id, string loggedInUser)
        {
            ViewBag.Username = loggedInUser;
            var contract = _dashboardService.GetContractDetailsWithLookups(id);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        [HttpPost]
        public IActionResult AcceptRequest(int id, string loggedInUser)
        {
            bool success = _dashboardService.AcceptContractRequest(id, loggedInUser);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }

        [HttpPost]
        public IActionResult DeclineRequest(int id, string loggedInUser)
        {
            bool success = _dashboardService.DeclineContractRequest(id, loggedInUser);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}