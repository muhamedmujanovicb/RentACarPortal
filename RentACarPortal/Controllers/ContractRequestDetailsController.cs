using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACarPortal.Controllers
{
    [Authorize(Roles = "Company")]

    public class ContractRequestDetailsController : Controller
    {
        private readonly DashboardService _dashboardService;

        public ContractRequestDetailsController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult ContractRequestDetails(int id)
        {
            var loggedInUser = User.Identity!.Name!;
            ViewBag.Username = loggedInUser;
            var (contract, vehicle) = _dashboardService.GetContractRequestDetails(id, loggedInUser);

            if (contract == null)
            {
                return NotFound();
            }

            ViewBag.Vehicle = vehicle;
            return View("ContractRequestDetails", contract);
        }

        [HttpPost]
        public IActionResult AcceptRequest(int id)
        {
            var loggedInUser = User.Identity!.Name!;
            bool success = _dashboardService.AcceptContractRequest(id, loggedInUser);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }

        [HttpPost]
        public IActionResult DeclineRequest(int id)
        {
            var loggedInUser = User.Identity!.Name!;
            bool success = _dashboardService.DeclineContractRequest(id, loggedInUser);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("Dashboard", "Dashboard", new { loggedInUser = loggedInUser });
        }
    }
}