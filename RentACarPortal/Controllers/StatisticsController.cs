using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;

namespace RentACarPortal.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly AppDbContext _context;

        public StatisticsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string loggedInUser)
        {
            string currentUsername = loggedInUser ?? User.Identity?.Name ?? "Admin";
            ViewBag.Username = currentUsername;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);
            if (user == null)
            {
                return View("~/Views/Dashboard/Statistics.cshtml");
            }

            var userVehicles = await _context.Vehicles
                .Where(v => v.UserId == user.Id)
                .ToListAsync();

            int totalFleetCount = userVehicles.Count;

            var userContracts = await _context.Contracts
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            // --- STAT 1: Fleet Utilization Over Time ---
            var utilizationByMonth = userContracts
                .GroupBy(c => new { c.RentalStartDate.Year, c.RentalStartDate.Month })
                .Select(g => new {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    RentedCount = g.Select(c => c.RegisterNumberOfVehicle).Distinct().Count()
                })
                .OrderBy(x => x.Period)
                .ToList();

            double overallUtilization = totalFleetCount > 0 ? ((double)(userContracts.Select(c => c.RegisterNumberOfVehicle).Distinct().Count()) / totalFleetCount) * 100 : 0;

            ViewBag.UtilizationMonths = utilizationByMonth.Any() ? utilizationByMonth.Select(x => x.Period).ToArray() : new[] { "Current Fleet" };
            ViewBag.UtilizationValues = utilizationByMonth.Any() ? utilizationByMonth.Select(x => totalFleetCount > 0 ? Math.Round(((double)x.RentedCount / totalFleetCount) * 100, 1) : 0.0).ToArray() : new[] { Math.Round(overallUtilization, 1) };

            // --- STAT 2: Top Revenue Source ---
            var revenueByType = userVehicles
                .Join(userContracts,
                    vehicle => vehicle.RegisterNumberOfVehicle,
                    contract => contract.RegisterNumberOfVehicle,
                    (vehicle, contract) => new {
                        Type = vehicle.TypeOfVehicle ?? "Standard",
                        Revenue = Math.Max(1, contract.RentalEndDate.DayNumber - contract.RentalStartDate.DayNumber) * vehicle.DailyRate
                    })
                .GroupBy(x => x.Type)
                .Select(g => new {
                    Type = g.Key,
                    TotalRevenue = g.Sum(x => x.Revenue)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(5)
                .ToList();

            ViewBag.RevenueLabels = revenueByType.Select(x => x.Type).ToArray();
            ViewBag.RevenueValues = revenueByType.Select(x => x.TotalRevenue).ToArray();

            // --- STAT 3: Most Popular Vehicles (Calculated by Rental Count) ---
            var popularVehicles = userVehicles
                .GroupJoin(userContracts,
                    vehicle => vehicle.RegisterNumberOfVehicle,
                    contract => contract.RegisterNumberOfVehicle,
                    (vehicle, contracts) => new {
                        VehicleName = $"{vehicle.Make} {vehicle.Model}",
                        RentalCount = contracts.Count()
                    })
                .OrderByDescending(x => x.RentalCount)
                .Take(5)
                .ToList();

            ViewBag.PopularLabels = popularVehicles.Select(x => x.VehicleName).ToArray();
            ViewBag.PopularValues = popularVehicles.Select(x => (double)x.RentalCount).ToArray();

            // --- STAT 4: Average Rental Duration ---
            var avgDurationByType = userContracts
                .Join(userVehicles,
                    contract => contract.RegisterNumberOfVehicle,
                    vehicle => vehicle.RegisterNumberOfVehicle,
                    (contract, vehicle) => new {
                        RentLength = (double)Math.Max(1, contract.RentalEndDate.DayNumber - contract.RentalStartDate.DayNumber),
                        Type = vehicle.TypeOfVehicle ?? "Standard"
                    })
                .GroupBy(x => x.Type)
                .Select(g => new {
                    Type = g.Key,
                    AvgDuration = g.Average(x => x.RentLength)
                })
                .ToList();

            ViewBag.DurationLabels = avgDurationByType.Select(x => x.Type).ToArray();
            ViewBag.DurationValues = avgDurationByType.Select(x => Math.Round(x.AvgDuration, 1)).ToArray();

            return View("~/Views/Dashboard/Statistics.cshtml");
        }
    }
}
