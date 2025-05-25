using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BirdLab.Models
{
    public class Observation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int BirdId { get; set; }
        
        [Required]
        public int LocationId { get; set; }
        
        [Required]
        public DateTime ObservationDate { get; set; }
        
        [Range(1, int.MaxValue)]
        public int Count { get; set; } = 1;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        [StringLength(100)]
        public string? ObserverName { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Bird Bird { get; set; } = null!;
        public Location Location { get; set; } = null!;
    }
}