using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class LoginController : Controller
    {
        private readonly HotelContext _context;

        public LoginController(HotelContext context)
        {
            _context = context;
        }

  
        [HttpGet]
        public IActionResult Index()
        {
            return View("Login");
        } 

        [HttpPost("api/Register/register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        [HttpPost("api/login/login")]
        public async Task<ActionResult> Login([FromBody] User user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);

            if (existingUser != null)
            {
                return Ok(new { message = "Login exitoso", email = existingUser.Email });
            }

            return Unauthorized(new { error = "Usuario o contraseña incorrectos" });
        }
    }
}
