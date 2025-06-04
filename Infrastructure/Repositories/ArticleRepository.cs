using Microsoft.EntityFrameworkCore;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Application.Interfaces;
using FengShuiWeb.Infrastructure.Data;

namespace FengShuiWeb.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly FengShuiDbContext _context;

        public ArticleRepository(FengShuiDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Article article)
        {
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task<Article> GetByIdAsync(int articleId)
        {
            return await _context.Articles.FirstOrDefaultAsync(a => a.ArticleId == articleId);
        }

        public async Task<List<Article>> GetPendingArticlesAsync()
        {
            return await _context.Articles
                .Where(a => a.Status == "Pending")
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<List<Article>> GetApprovedArticlesAsync()
        {
            return await _context.Articles
                .Where(a => a.Status == "Approved")
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<List<ArticleHistory>> GetArticleHistoryAsync(int articleId)
        {
            return await _context.ArticleHistory
                .Where(ah => ah.ArticleId == articleId)
                .OrderByDescending(ah => ah.EditedDate)
                .ToListAsync();
        }

        public async Task CreateReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            return await _context.Reports
                .Include(r => r.Article)
                .Include(r => r.Reporter)
                .FirstOrDefaultAsync(r => r.ReportId == reportId);
        }

        public async Task CreateWarningAsync(Warning warning)
        {
            await _context.Warnings.AddAsync(warning);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Warnings)
                .Include(u => u.Articles)
                    .ThenInclude(a => a.Reports)
                .FirstOrDefaultAsync(u => u.UserID == userId);
        }
    }
}   