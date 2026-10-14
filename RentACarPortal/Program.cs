using Microsoft.EntityFrameworkCore;
using RentACarPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RentACarPortal.Data.AppDbContext>(options => options.UseSqlite("Data Source=fleetdrive.db"));
builder.Services.AddScoped<BookingManager>();
builder.Services.AddScoped<NotificationManager>();
builder.Services.AddScoped<FleetService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<StatisticsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RentACarPortal.Data.AppDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
