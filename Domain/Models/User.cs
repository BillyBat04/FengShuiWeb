using System;
using System.ComponentModel.DataAnnotations;

namespace FengShuiWeb.Domain.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsEmailConfirmed { get; set; } = false;
        public string Role { get; set; } = "User";
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;
        public List<FengShuiAnalysis> FengShuiAnalyses { get; set; } = new List<FengShuiAnalysis>();
        public int ReportCount { get; set; } = 0; // Số lần bị báo cáo
        public string ReportReasons { get; set; } = ""; // Lý do báo cáo, lưu dạng chuỗi phân tách      
        public List<Article> Articles { get; set; }
        public List<Report> Reports { get; set; }
        public List<Warning> Warnings { get; set; }
        public int WarningCount { get; set; } = 0;
        public bool IsBanned { get; set; } = false;
        public DateTime? BanExpirationDate { get; set; }    
    }
}
    