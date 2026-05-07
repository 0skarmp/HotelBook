using System.Text.Json.Serialization;

namespace Hotel.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = "Libre"; 
        [JsonIgnore]
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }

    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Passport { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public string Status { get; set; } = "ocupado"; 
        public string RoomNumber => Room?.Number.ToString() ?? string.Empty;
        public string RoomType => Room?.Type ?? string.Empty;
    }
}
