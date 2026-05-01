using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly HotelContext _context;

        public ReservationsController(HotelContext context)
        {
            _context = context;
        }

        // 🔹 CREAR RESERVA
        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
        {
            try
            {
                // ✅ Validaciones básicas
                if (reservation.RoomId == 0)
                    return BadRequest("RoomId es requerido");

                if (reservation.EntryDate >= reservation.ExitDate)
                    return BadRequest("La fecha de salida debe ser mayor a la de entrada");

                var room = await _context.Rooms
                    .Include(r => r.Reservations)
                    .FirstOrDefaultAsync(r => r.Id == reservation.RoomId);

                if (room == null)
                    return NotFound("Room not found");

                // 🔥 VALIDAR DISPONIBILIDAD (no permitir solapamientos)
                var hasConflict = room.Reservations.Any(r =>
                    reservation.EntryDate < r.ExitDate &&
                    reservation.ExitDate > r.EntryDate
                );

                if (hasConflict)
                    return BadRequest("La habitación ya está reservada en ese rango de fechas");

                // 🔹 Guardar reserva
                _context.Reservations.Add(reservation);

                // 🔹 (Opcional) actualizar estado
                room.Status = "Ocupada";

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // 🔹 OBTENER TODAS LAS RESERVAS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .ToListAsync();
        }

        // 🔹 OBTENER RESERVA POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            return reservation;
        }

        // 🔹 CHECKOUT (LIBERAR HABITACIÓN)
        [HttpPut("{id}/checkout")]
        public async Task<IActionResult> Checkout(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            // liberar habitación
            reservation.Room.Status = "Libre";

            // eliminar reserva (o podrías marcarla como finalizada)
            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🔹 ELIMINAR RESERVA
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
                return NotFound();

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}