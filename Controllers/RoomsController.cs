using Hotel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly HotelContext _context;

        public RoomsController(HotelContext context)
        {
            _context = context;
        }

        [HttpGet]
        public string Test()
        {
            return "Hello from RoomsController!";
        }

        //Endpoint para guardar una habitacion
        [HttpPost]
        public async Task<ActionResult<Room>> Save(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Save), new { id = room.Id }, room);
        }

    }
}
