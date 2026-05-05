using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    public class MaintenanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
