using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BirdLab.Models
{
    public class BirdDetails
    {
        [Key]
        [ForeignKey("Bird")]
        public int BirdId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Info { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Range(0, 100)]
        public double? AverageLength { get; set; } // in cm
        
        [Range(0, 10000)]
        public double? AverageWeight { get; set; } // in grams
        
        [StringLength(200)]
        public string? Diet { get; set; }
        
        [StringLength(200)]
        public string? Behavior { get; set; }
        
        public bool IsEndangered { get; set; } = false;
        
        // Navigation property back to Bird
        public Bird Bird { get; set; } = null!;
    }
}