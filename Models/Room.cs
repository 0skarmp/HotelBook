namespace Hotel.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public string Status { get; set; } = "Libre"; // por defecto libre

        // Relación con reservas
        public ICollection<Reservation> Reservations { get; set; }
    }

    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public string GuestName { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public string Plate { get; set; }
    }
}
