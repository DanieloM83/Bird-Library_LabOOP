using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BirdLab.Models
{
    // Many-to-many relationship between Bird and Location (habitats)
    public class BirdHabitat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int BirdId { get; set; }
        
        [Required]
        public int LocationId { get; set; }
        
        [StringLength(50)]
        public string? Season { get; set; } // Spring, Summer, Fall, Winter
        
        public bool IsPrimaryHabitat { get; set; } = false;
        
        // Navigation properties
        public Bird Bird { get; set; } = null!;
        public Location Location { get; set; } = null!;
    }
}