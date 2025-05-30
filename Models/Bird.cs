using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BirdLab.Models
{
    public class Bird
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public Species Species { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation property for one-to-one relationship
        public BirdDetails? BirdDetails { get; set; }
        
        // Navigation properties for other relationships
        public ICollection<Observation> Observations { get; set; } = new List<Observation>();
        public ICollection<BirdHabitat> BirdHabitats { get; set; } = new List<BirdHabitat>();
    }
}