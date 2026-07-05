using Microsoft.AspNetCore.Mvc;

namespace RentACarPortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
