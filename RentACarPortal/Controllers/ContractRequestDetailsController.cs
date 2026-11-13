using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACarPortal.Controllers
{
    [Authorize(Roles = "Company")]

    public class ContractRequestDetailsController : Controller
    {
        private readonly DashboardService _dashboardService;
        private readonly NotificationManager _notificationManager;

        public ContractRequestDetailsController(DashboardService dashboardService, NotificationManager notificationManager)
        {
            _dashboardService = dashboardService;
            _notificationManager = notificationManager;
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
        public async Task<IActionResult> AcceptRequest(int id, string? remarks)
        {
            var loggedInUser = User.Identity!.Name!;
            var emailData = _dashboardService.AcceptContractRequest(id, loggedInUser, remarks);
            if (emailData == null)
            {
                return NotFound();
            }

            await _notificationManager.SendRequestAcceptedAsync(emailData);

            return RedirectToAction("Dashboard", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DeclineRequest(int id, string? remarks)
        {
            var loggedInUser = User.Identity!.Name!;
            var emailData = _dashboardService.DeclineContractRequest(id, loggedInUser, remarks);
            if (emailData == null)
            {
                return NotFound();
            }

            await _notificationManager.SendRequestDeclinedAsync(emailData);

            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
}