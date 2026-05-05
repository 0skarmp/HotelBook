using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class Report
    {
        public int Id { get; set; }
        
        [Required]
        public string? UserId { get; set; }
        
        [Required]
        public string? UserName { get; set; }
        
        [Required]
        public string? ActionType { get; set; } // "Create", "Edit", "Delete"
        
        public int? ReservationId { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        public string? Details { get; set; }
    }
}