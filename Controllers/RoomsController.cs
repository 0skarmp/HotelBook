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
            var rooms = await _context.Rooms
                .Include(r => r.Reservations)
                .ToListAsync();
            
            var today = DateTime.Today;

            foreach (var room in rooms)
            {
                var hasActiveReservation = room.Reservations
                    .Any(r => r.EntryDate <= today && r.ExitDate > today);
                room.Status = hasActiveReservation ? "Ocupada" : "Libre";
            }

            return Ok(rooms);
        }

        // API: crear habitación
        [HttpPost("/api/rooms")]
        public async Task<ActionResult<Room>> CreateRoom([FromBody] Room room)
        {
            // Validaciones
            if (room == null)
            {
                return BadRequest(new { error = "Los datos de la habitación son requeridos" });
            }

            if (room.Number <= 0)
            {
                return BadRequest(new { error = "El número de habitación debe ser mayor a 0" });
            }

            if (string.IsNullOrEmpty(room.Type))
            {
                return BadRequest(new { error = "El tipo de habitación es requerido" });
            }

            // Verificar si ya existe una habitación con el mismo número
            var existingRoom = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Number == room.Number);
            
            if (existingRoom != null)
            {
                return Conflict(new { error = $"Ya existe una habitación con el número {room.Number}" });
            }

            room.Status = "Libre";
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, room);
        }

        // API: eliminar habitación
        [HttpDelete("/api/rooms/{id}")]
        public async Task<ActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            
            if (room == null)
            {
                return NotFound(new { error = "Habitación no encontrada" });
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}