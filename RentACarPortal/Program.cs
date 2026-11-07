using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarPortal.Data;
using RentACarPortal.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/LoginForm";
        options.AccessDeniedPath = "/Login/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = false;                       // 8h counted from login, no renewals
        options.Cookie.HttpOnly = true;                          
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
        options.Cookie.SameSite = SameSiteMode.Lax;              // samesite mode LAX

        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async context =>
            {
                var idClaim = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = context.Principal?.FindFirst(ClaimTypes.Role)?.Value;

                bool valid = false;
                if (int.TryParse(idClaim, out var id))
                {
                    var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

                    if (role == "Company")
                        valid = await db.Companies.AnyAsync(c => c.Id == id && c.IsEmailVerified);
                    else if (role == "Client")
                        valid = await db.Clients.AnyAsync(c => c.Id == id && c.IsEmailVerified);
                }

                if (!valid)
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
        };
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<RentACarPortal.Data.AppDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddScoped<BookingManager>();
builder.Services.AddScoped<NotificationManager>();
builder.Services.AddScoped<FleetService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<StatisticsService>();
builder.Services.AddScoped<IEmailSender, GmailEmailSender>();

builder.Services.AddSingleton<PasswordService>();
builder.Services.AddSingleton<TokenService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
