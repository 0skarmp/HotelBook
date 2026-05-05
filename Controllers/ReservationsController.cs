using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    public class ReservationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
