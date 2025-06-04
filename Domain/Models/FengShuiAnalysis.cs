using System.ComponentModel.DataAnnotations;

namespace FengShuiWeb.Domain.Models
{
    public class FengShuiAnalysis
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserID { get; set; }

        public User User { get; set; }

        [Required]
        [StringLength(100)]
        public string Label { get; set; } // e.g., "Current Home"

        [Required]
        public string AnalysisData { get; set; } // JSON string containing analysis result

        public DateTime CreatedAt { get; set; }
    }
}
