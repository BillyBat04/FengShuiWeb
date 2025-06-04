using System.Collections.Generic;
using System.Threading.Tasks;
using FengShuiWeb.Application.DTOs;

namespace FengShuiWeb.Application.Interfaces
{
    public interface IArticleService
    {
        Task<int> CreateArticleAsync(int userId, CreateArticleDto dto);
        Task UpdateArticleAsync(int userId, int articleId, UpdateArticleDto dto);
        Task<List<ArticleDto>> GetPendingArticlesAsync();
        Task ApproveArticleAsync(int articleId);
        Task RejectArticleAsync(int articleId, string reason);
        Task<List<ArticleHistoryDto>> GetArticleHistoryAsync(int userId, int articleId);
        Task<int> ReportArticleAsync(int? userId, string reporterEmail, int articleId, CreateReportDto dto);
        Task ResolveReportAsync(int reportId, bool isViolation, string reason);
        Task<UserReportDto> GetUserReportsAsync(int userId);
        Task<List<ArticleDto>> GetApprovedArticlesAsync();
    }
}