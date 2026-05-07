using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    public class ProfileController : Controller
    {
        [HttpGet("/Profile")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
