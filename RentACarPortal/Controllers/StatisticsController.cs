using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACarPortal.Controllers
{
    [Authorize(Roles = "Company")]

    public class StatisticsController : Controller
    {
        private readonly StatisticsService _statisticsService;

        public StatisticsController(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUser = User.Identity!.Name!;
            string currentUsername = User.Identity!.Name!;
            ViewBag.Username = currentUsername;

            var stats = await _statisticsService.GetStatisticsDataAsync(currentUsername);
            if (stats == null)
            {
                return View("Statistics");
            }

            ViewBag.UtilizationMonths = stats.UtilizationMonths;
            ViewBag.UtilizationValues = stats.UtilizationValues;
            ViewBag.RevenueLabels = stats.RevenueLabels;
            ViewBag.RevenueValues = stats.RevenueValues;
            ViewBag.PopularLabels = stats.PopularLabels;
            ViewBag.PopularValues = stats.PopularValues;
            ViewBag.DurationLabels = stats.DurationLabels;
            ViewBag.DurationValues = stats.DurationValues;

            return View("Statistics");
        }
    }
}