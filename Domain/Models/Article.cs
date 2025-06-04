using FengShuiWeb.Domain.Models;
using System.ComponentModel.DataAnnotations;

public class Article
{
    public int ArticleId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public string Tags { get; set; }
    [Required]
    public string References { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    public int UserID { get; set; }
    public User User { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public List<ArticleHistory> History { get; set; }
    public List<Report> Reports { get; set; }
}