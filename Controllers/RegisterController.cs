using Hotel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    public class RegisterController : Controller
    {
        private readonly HotelContext _context;

        public RegisterController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Register");
        }

        [HttpPost("api/register/create")]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
        }
    }
}
