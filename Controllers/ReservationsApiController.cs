using Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationsApiController : ControllerBase
    {
        private readonly HotelContext _context;

        public ReservationsApiController(HotelContext context)
        {
            _context = context;
        }

        private async Task LogReport(string actionType, int? reservationId, string details = null)
        {
            // Si tienes autenticación propia, aquí puedes obtener el usuario actual
            var userName = User.Identity?.Name ?? "Unknown";

            var report = new Report
            {
                UserId = "system", // puedes cambiarlo si manejas Id de usuario
                UserName = userName,
                ActionType = actionType,
                ReservationId = reservationId,
                Date = DateTime.Now,
                Details = details
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
        }

        // CREAR RESERVA
        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
        {
            if (reservation.RoomId == 0)
                return BadRequest("RoomId es requerido");

            if (reservation.EntryDate >= reservation.ExitDate)
                return BadRequest("La fecha de salida debe ser mayor a la de entrada");

            var room = await _context.Rooms
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Id == reservation.RoomId);

            if (room == null)
                return NotFound("Room not found");

            var hasConflict = room.Reservations.Any(r =>
                reservation.EntryDate < r.ExitDate &&
                reservation.ExitDate > r.EntryDate
            );

            if (hasConflict)
                return BadRequest("La habitación ya está reservada en ese rango de fechas");

            _context.Reservations.Add(reservation);
            room.Status = "Ocupada";

            await _context.SaveChangesAsync();
            await LogReport("Create", reservation.Id, $"Reserva creada para habitación {room.Number}");

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        // OBTENER TODAS LAS RESERVAS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .ToListAsync();
        }

        // OBTENER REPORTES DEL DÍA
        [HttpGet("reports/today")]
        public async Task<ActionResult<IEnumerable<Report>>> GetTodayReports()
        {
            var today = DateTime.Today;
            return await _context.Reports
                .Where(r => r.Date.Date == today)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        // OBTENER RESERVA POR ID
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

        // CHECKOUT (LIBERAR HABITACIÓN)
        [HttpPut("{id}/checkout")]
        public async Task<IActionResult> Checkout(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            if (reservation.Room != null)
            {
                reservation.Room.Status = "Libre";
            }
            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync();
            await LogReport("Checkout", id, $"Reserva finalizada y habitación liberada");

            return NoContent();
        }

        // ELIMINAR RESERVA
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
                return NotFound();

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            await LogReport("Delete", id, $"Reserva eliminada");

            return NoContent();
        }
    }
}
