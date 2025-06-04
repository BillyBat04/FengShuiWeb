using System.Collections.Generic;
using System.Threading.Tasks;
using FengShuiWeb.Domain.Models;

namespace FengShuiWeb.Application.Interfaces
{
    public interface IArticleRepository
    {
        Task CreateAsync(Article article);
        Task UpdateAsync(Article article);
        Task<Article> GetByIdAsync(int articleId);
        Task<List<Article>> GetPendingArticlesAsync();
        Task<List<ArticleHistory>> GetArticleHistoryAsync(int articleId);
        Task CreateReportAsync(Report report);
        Task<Report> GetReportByIdAsync(int reportId);
        Task CreateWarningAsync(Warning warning);
        Task<User> GetUserByIdAsync(int userId);
        Task<List<Article>> GetApprovedArticlesAsync();
    }
}   