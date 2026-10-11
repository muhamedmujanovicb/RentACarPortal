using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Models;

namespace RentACarPortal.Services
{
    public class StatisticsService
    {
        private readonly AppDbContext _context;

        public StatisticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StatisticsDataDto?> GetStatisticsDataAsync(string currentUsername)
        {
            var user = await _context.Companies.FirstOrDefaultAsync(u => u.Username == currentUsername);
            if (user == null)
            {
                return null;
            }

            // Eager load normalized Make and Model lookup tables
            var userVehicles = await _context.Vehicles
                .Include(v => v.VehicleMake)
                .Include(v => v.VehicleModel)
                .Include(v => v.VehicleType)
                .Where(v => v.CompanyId == user.Id)
                .ToListAsync();

            int totalFleetCount = userVehicles.Count;

            var userContracts = await _context.Contracts
                .Where(c => c.ClientId == user.Id)
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

            var utilizationMonths = utilizationByMonth.Any() ? utilizationByMonth.Select(x => x.Period).ToArray() : new[] { "Current Fleet" };
            var utilizationValues = utilizationByMonth.Any() ? utilizationByMonth.Select(x => totalFleetCount > 0 ? Math.Round(((double)x.RentedCount / totalFleetCount) * 100, 1) : 0.0).ToArray() : new[] { Math.Round(overallUtilization, 1) };

            // --- STAT 2: Top Revenue Source ---
            var revenueByType = userVehicles
                .Join(userContracts,
                    vehicle => vehicle.RegisterNumberOfVehicle,
                    contract => contract.RegisterNumberOfVehicle,
                    (vehicle, contract) => new {
                        // Access the relational VehicleType name safely
                        Type = vehicle.VehicleType?.Name ?? "Standard",
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

            var revenueLabels = revenueByType.Select(x => x.Type).ToArray();
            var revenueValues = revenueByType.Select(x => x.TotalRevenue).ToArray();

            // --- STAT 3: Most Popular Vehicles (Calculated by Rental Count) ---
            var popularVehicles = userVehicles
                .GroupJoin(userContracts,
                    vehicle => vehicle.RegisterNumberOfVehicle,
                    contract => contract.RegisterNumberOfVehicle,
                    (vehicle, contracts) => new {
                        // Safely resolve normalized relational Make and Model names
                        VehicleName = vehicle.VehicleMake != null && vehicle.VehicleModel != null
                            ? $"{vehicle.VehicleMake.Name} {vehicle.VehicleModel.Name}"
                            : $"{vehicle.VehicleMake} {vehicle.VehicleModel}",
                        RentalCount = contracts.Count()
                    })
                .OrderByDescending(x => x.RentalCount)
                .Take(5)
                .ToList();

            var popularLabels = popularVehicles.Select(x => x.VehicleName).ToArray();
            var popularValues = popularVehicles.Select(x => (double)x.RentalCount).ToArray();

            // --- STAT 4: Average Rental Duration ---
            var avgDurationByType = userContracts
                .Join(userVehicles,
                    contract => contract.RegisterNumberOfVehicle,
                    vehicle => vehicle.RegisterNumberOfVehicle,
                    (contract, vehicle) => new {
                        RentLength = (double)Math.Max(1, contract.RentalEndDate.DayNumber - contract.RentalStartDate.DayNumber),
                        // Access the relational VehicleType name safely
                        Type = vehicle.VehicleType?.Name ?? "Standard"
                    })
                .GroupBy(x => x.Type)
                .Select(g => new {
                    Type = g.Key,
                    AvgDuration = g.Average(x => x.RentLength)
                })
                .ToList();

            var durationLabels = avgDurationByType.Select(x => x.Type).ToArray();
            var durationValues = avgDurationByType.Select(x => Math.Round(x.AvgDuration, 1)).ToArray();

            return new StatisticsDataDto
            {
                UtilizationMonths = utilizationMonths,
                UtilizationValues = utilizationValues,
                RevenueLabels = revenueLabels,
                RevenueValues = revenueValues,
                PopularLabels = popularLabels,
                PopularValues = popularValues,
                DurationLabels = durationLabels,
                DurationValues = durationValues
            };
        }

        public class StatisticsDataDto
        {
            public string[] UtilizationMonths { get; set; } = Array.Empty<string>();
            public double[] UtilizationValues { get; set; } = Array.Empty<double>();
            public string[] RevenueLabels { get; set; } = Array.Empty<string>();
            public double[] RevenueValues { get; set; } = Array.Empty<double>();
            public string[] PopularLabels { get; set; } = Array.Empty<string>();
            public double[] PopularValues { get; set; } = Array.Empty<double>();
            public string[] DurationLabels { get; set; } = Array.Empty<string>();
            public double[] DurationValues { get; set; } = Array.Empty<double>();
        }
    }
}

