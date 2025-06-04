namespace FengShuiWeb.Domain.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int? ReporterId { get; set; } // Có thể null nếu không đăng nhập
        public User Reporter { get; set; }
        public string ReporterEmail { get; set; } // Lưu email nếu không đăng nhập
        public string Reason { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Resolved
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
