using System.ComponentModel.DataAnnotations;

namespace FengShuiWeb.Domain.Models
{
    public class ArticleHistory
    {
        [Key]
        public int HistoryId { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Tags { get; set; }
        public string References { get; set; }
        public DateTime EditedDate { get; set; } = DateTime.UtcNow;
    }
}
