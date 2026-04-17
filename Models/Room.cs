namespace Hotel.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public string Type { get; set; }

        public decimal price { get; set; }
        public bool Available { get; set; }
    }
}
