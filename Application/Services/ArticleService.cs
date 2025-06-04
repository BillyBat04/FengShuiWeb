using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Application.DTOs;
using FengShuiWeb.Application.Interfaces;
using FengShuiWeb.Infrastructure.Data;

namespace FengShuiWeb.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly FengShuiDbContext _context;

        public ArticleService(
            IArticleRepository articleRepository,
            IUserRepository userRepository,
            IEmailService emailService,
            IMapper mapper,
            FengShuiDbContext context)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> CreateArticleAsync(int userId, CreateArticleDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Người dùng không tồn tại.");
            if (user.IsBanned && user.BanExpirationDate > DateTime.UtcNow)
                throw new UnauthorizedAccessException($"Bạn bị cấm đăng bài đến {user.BanExpirationDate}");

            var article = _mapper.Map<Article>(dto);
            article.UserID = userId;
            article.Status = "Pending";

            await _articleRepository.CreateAsync(article);
            return article.ArticleId;
        }

        public async Task UpdateArticleAsync(int userId, int articleId, UpdateArticleDto dto)
        {
            var article = await _articleRepository.GetByIdAsync(articleId);
            if (article == null)
                throw new KeyNotFoundException("Bài đăng không tồn tại.");
            if (article.UserID != userId)
                throw new UnauthorizedAccessException("Bạn không có quyền chỉnh sửa bài này.");

            var history = new ArticleHistory
            {
                ArticleId = articleId,
                Title = article.Title,
                Content = article.Content,
                ImageUrl = article.ImageUrl,
                Tags = article.Tags,
                References = article.References,
                EditedDate = DateTime.UtcNow
            };
            await _context.ArticleHistory.AddAsync(history);

            _mapper.Map(dto, article);
            article.Status = "Pending";
            await _articleRepository.UpdateAsync(article);
        }

        public async Task<List<ArticleDto>> GetPendingArticlesAsync()
        {
            var articles = await _articleRepository.GetPendingArticlesAsync();
            return _mapper.Map<List<ArticleDto>>(articles);
        }

        public async Task<List<ArticleDto>> GetApprovedArticlesAsync()
        {
            var articles = await _articleRepository.GetApprovedArticlesAsync();
            return _mapper.Map<List<ArticleDto>>(articles);
        }

        public async Task ApproveArticleAsync(int articleId)
        {
            var article = await _articleRepository.GetByIdAsync(articleId);
            if (article == null)
                throw new KeyNotFoundException("Bài đăng không tồn tại.");

            article.Status = "Approved";
            await _articleRepository.UpdateAsync(article);

            var user = await _userRepository.GetByIdAsync(article.UserID);
            await _emailService.SendEmailAsync(user.Email, "Bài đăng được duyệt",
                $"Bài đăng '{article.Title}' của bạn đã được duyệt và hiển thị trên trang web.");
        }

        public async Task RejectArticleAsync(int articleId, string reason)
        {
            var article = await _articleRepository.GetByIdAsync(articleId);
            if (article == null)
                throw new KeyNotFoundException("Bài đăng không tồn tại.");

            article.Status = "Rejected";
            await _articleRepository.UpdateAsync(article);

            var user = await _userRepository.GetByIdAsync(article.UserID);
            await _emailService.SendEmailAsync(user.Email, "Bài đăng bị từ chối",
                $"Bài đăng '{article.Title}' của bạn bị từ chối vì: {reason}");
        }

        public async Task<List<ArticleHistoryDto>> GetArticleHistoryAsync(int userId, int articleId)
        {
            var article = await _articleRepository.GetByIdAsync(articleId);
            if (article == null)
                throw new KeyNotFoundException("Bài đăng không tồn tại.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || (article.UserID != userId && user.Role != "Admin"))
                throw new UnauthorizedAccessException("Bạn không có quyền xem lịch sử chỉnh sửa.");

            var history = await _articleRepository.GetArticleHistoryAsync(articleId);
            return _mapper.Map<List<ArticleHistoryDto>>(history);
        }

        public async Task<int> ReportArticleAsync(int? userId, string reporterEmail, int articleId, CreateReportDto dto)
        {
            var article = await _articleRepository.GetByIdAsync(articleId);
            if (article == null)
                throw new KeyNotFoundException("Bài đăng không tồn tại.");

            var report = _mapper.Map<Report>(dto);
            report.ArticleId = articleId;
            report.ReporterId = userId;
            report.ReporterEmail = reporterEmail;

            await _articleRepository.CreateReportAsync(report);

            var author = await _userRepository.GetByIdAsync(article.UserID);
            author.ReportCount++;
            author.ReportReasons = string.IsNullOrEmpty(author.ReportReasons)
                ? dto.Reason
                : $"{author.ReportReasons};{dto.Reason}";
            await _userRepository.UpdateAsync(author);

            return report.ReportId;
        }

        public async Task ResolveReportAsync(int reportId, bool isViolation, string reason)
        {
            var report = await _articleRepository.GetReportByIdAsync(reportId);
            
            if (report == null)
                throw new KeyNotFoundException("Báo cáo không tồn tại.");

            report.Status = "Resolved";
            _context.Reports.Update(report); 
            await _context.SaveChangesAsync();

            var article = await _articleRepository.GetByIdAsync(report.ArticleId);
            var reporterEmail = report.ReporterEmail;
            if (string.IsNullOrEmpty(reporterEmail) && report.ReporterId.HasValue)
            {
                var reporter = await _userRepository.GetByIdAsync(report.ReporterId.Value);
                reporterEmail = reporter?.Email;
            }
            if (string.IsNullOrEmpty(reason)) throw new ArgumentException("Lý do xử lý báo cáo không được để trống");
            if (!isViolation)
            {
                if (!string.IsNullOrEmpty(reporterEmail))
                {
                    await _emailService.SendEmailAsync(reporterEmail, "Báo cáo không vi phạm",
                        $"Báo cáo về bài đăng '{article.Title}' đã được xem xét và không vi phạm.");
                }
                return;
            }

            var author = await _userRepository.GetByIdAsync(article.UserID);
            var warning = new Warning
            {
                UserID = article.UserID,
                ArticleId = report.ArticleId, 
                Reason = reason,
                CreatedDate = DateTime.UtcNow
            };
            await _articleRepository.CreateWarningAsync(warning);

            author.WarningCount++;
            if (author.WarningCount >= 3)
            {
                author.IsBanned = true;
                author.BanExpirationDate = DateTime.UtcNow.AddMonths(1);
            }
            await _userRepository.UpdateAsync(author);

            article.Status = "Rejected";
            await _articleRepository.UpdateAsync(article);

            await _emailService.SendEmailAsync(author.Email, "Cảnh cáo bài đăng",
                $"Bài đăng '{article.Title}' của bạn vi phạm vì: {reason}. Bạn đã bị cảnh cáo {author.WarningCount} lần.");
        }

        public async Task<UserReportDto> GetUserReportsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Người dùng không tồn tại.");

            return _mapper.Map<UserReportDto>(user);
        }
    }
}