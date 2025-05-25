using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BirdLab.Models
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }
        
        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }
        
        [StringLength(100)]
        public string? Country { get; set; }
        
        [StringLength(100)]
        public string? Region { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        // Navigation properties
        public ICollection<Observation> Observations { get; set; } = new List<Observation>();
        public ICollection<BirdHabitat> BirdHabitats { get; set; } = new List<BirdHabitat>();
    }
}