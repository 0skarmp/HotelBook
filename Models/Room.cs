namespace Hotel.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = "Libre"; // por defecto libre

        // Relación con reservas (inicializada para evitar error 400)
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }

    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public string GuestName { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public string Plate { get; set; } = string.Empty;
    }
}
