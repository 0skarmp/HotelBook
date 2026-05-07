using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    public class GuestsController : Controller
    {
        private readonly HotelContext _context;

        public GuestsController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet("/Guests")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/api/guests")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetGuests()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Room)
                .OrderByDescending(r => r.EntryDate)
                .ToListAsync();

            return reservations;
        }
    }
}
