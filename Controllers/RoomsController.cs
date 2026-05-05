using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Route("[controller]")]
    public class RoomsController : Controller
    {
        private readonly HotelContext _context;

        public RoomsController(HotelContext context)
        {
            _context = context;
        }

        // Vista principal
        [HttpGet]
        public IActionResult Index()
        {
            return View("Rooms");
        }

        // API: obtener habitaciones
        [HttpGet("/api/rooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            var rooms = await _context.Rooms.Include(r => r.Reservations).ToListAsync();
            var today = DateTime.Today;

            foreach (var room in rooms)
            {
                var hasActiveReservation = room.Reservations.Any(r => r.EntryDate <= today && r.ExitDate > today);
                room.Status = hasActiveReservation ? "Ocupada" : "Libre";
            }

            return rooms;
        }

        // API: crear habitación
        [HttpPost("/api/rooms")]
        public async Task<ActionResult<Room>> CreateRoom(Room room)
        {
            room.Status = "Libre";
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, room);
        }
    }
}
