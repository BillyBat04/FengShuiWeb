using System;
using System.ComponentModel.DataAnnotations;

namespace FengShuiWeb.Application.DTOs
{
    public class CreateArticleDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Tags { get; set; }
        [Required]
        public string References { get; set; }
    }

    public class UpdateArticleDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Tags { get; set; }
        [Required]
        public string References { get; set; }
    }

    public class ArticleDto
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Tags { get; set; }
        public string References { get; set; }
        public string Status { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ArticleHistoryDto
    {
        public int HistoryId { get; set; }
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Tags { get; set; }
        public string References { get; set; }
        public DateTime EditedDate { get; set; }
    }

    public class CreateReportDto
    {
        [Required]
        public string Reason { get; set; }
        public string ReporterEmail { get; set; }
    }

    public class ReportDto
    {
        public int ReportId { get; set; }
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public int? ReporterId { get; set; }
        public string ReporterName { get; set; }
        public string ReporterEmail { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class WarningDto
    {
        public int WarningId { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public int? ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class UserReportDto
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public int ReportCount { get; set; }
        public string ReportReasons { get; set; }
        public List<ReportDto> Reports { get; set; }
    }
}