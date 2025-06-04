namespace FengShuiWeb.Domain.Models
{
    public class Warning
    {
        public int WarningId { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int? ArticleId { get; set; }
        public Article Article { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
