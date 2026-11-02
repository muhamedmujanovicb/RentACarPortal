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
            //calls for dashboardservice to get all contract request details
            var (contract, vehicle) = _dashboardService.GetContractRequestDetails(id, loggedInUser);

            if (contract == null)
            {
                return NotFound();
            }

            return View("ContractRequestDetails", contract);
        }

        [HttpPost]
        public IActionResult AcceptRequest(int id)
        {
            var loggedInUser = User.Identity!.Name!;
            //calls dashboard service to accept contract request
            bool success = _dashboardService.AcceptContractRequest(id, loggedInUser);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("Dashboard", "Dashboard");
        }

        [HttpPost]
        public IActionResult DeclineRequest(int id)
        {
            var loggedInUser = User.Identity!.Name!;
            //calls dashboard service to decline contract request
            bool success = _dashboardService.DeclineContractRequest(id, loggedInUser);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
}