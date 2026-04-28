using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("Dashboard");
        }
    }
}
