using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RentACarPortal.Controllers
{
    public class ContractRequestDetailsController : Controller
    {
        [HttpGet]
        public IActionResult ContractRequestDetails()
        {
            return View();
        }
    }
}
