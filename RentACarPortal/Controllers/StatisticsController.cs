using Microsoft.AspNetCore.Mvc;
using RentACarPortal.Services;

namespace RentACarPortal.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly StatisticsService _statisticsService;

        public StatisticsController(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task<IActionResult> Index(string loggedInUser)
        {
            string currentUsername = loggedInUser ?? User.Identity?.Name ?? "Admin";
            ViewBag.Username = currentUsername;

            var stats = await _statisticsService.GetStatisticsDataAsync(currentUsername);
            if (stats == null)
            {
                return View("~/Views/Dashboard/Statistics.cshtml");
            }

            ViewBag.UtilizationMonths = stats.UtilizationMonths;
            ViewBag.UtilizationValues = stats.UtilizationValues;
            ViewBag.RevenueLabels = stats.RevenueLabels;
            ViewBag.RevenueValues = stats.RevenueValues;
            ViewBag.PopularLabels = stats.PopularLabels;
            ViewBag.PopularValues = stats.PopularValues;
            ViewBag.DurationLabels = stats.DurationLabels;
            ViewBag.DurationValues = stats.DurationValues;

            return View("~/Views/Dashboard/Statistics.cshtml");
        }
    }
}