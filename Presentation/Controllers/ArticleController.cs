using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FengShuiWeb.Application.Interfaces;
using FengShuiWeb.Application.DTOs;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;

namespace FengShuiWeb.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ArticleController(IArticleService articleService, IArticleRepository articleRepository, IMapper mapper, IMemoryCache cache)
        {
            _articleService = articleService;
            _articleRepository = articleRepository;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var articleId = await _articleService.CreateArticleAsync(userId, dto);
            return dto == null ? BadRequest("Dữ liệu bài viết không hợp lệ") : CreatedAtAction(nameof(GetArticle), new { id = articleId }, null);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] UpdateArticleDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _articleService.UpdateArticleAsync(userId, id, dto);
            if (dto == null) return BadRequest("Dữ liệu bài viết không hợp lệ");
            return NoContent();
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingArticles()
        {
            var articles = await _articleService.GetPendingArticlesAsync();
            return Ok(articles);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveArticle(int id)
        {
            await _articleService.ApproveArticleAsync(id);
            _cache.Remove("ApprovedArticles");
            return NoContent();
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectArticle(int id, [FromBody] string reason)
        {
            await _articleService.RejectArticleAsync(id, reason);
            if (string.IsNullOrEmpty(reason)) return BadRequest("Lý do từ chối không được để trống");   
            return NoContent();
        }

        [HttpGet("{id}/history")]
        [Authorize]
        public async Task<IActionResult> GetArticleHistory(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var history = await _articleService.GetArticleHistoryAsync(userId, id);
            return Ok(history);
        }

        [HttpPost("{id}/report")]
        public async Task<IActionResult> ReportArticle(int id, [FromBody] CreateReportDto dto)
        {
            int? userId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : null;
            var reportId = await _articleService.ReportArticleAsync(userId, dto.ReporterEmail, id, dto);
            if (dto == null || string.IsNullOrEmpty(dto.Reason)) return BadRequest("Lý do báo cáo không hợp lệ");
            return CreatedAtAction(nameof(GetReport), new { id = reportId }, null);
        }

        [HttpPut("reports/{id}/resolve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResolveReport(int id, [FromBody] ResolveReportDto dto)
        {
            await _articleService.ResolveReportAsync(id, dto.IsViolation, dto.Reason);
            if (dto == null || string.IsNullOrEmpty(dto.Reason)) return BadRequest("Dữ liệu xử lý báo cáo không hợp lệ");
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetApprovedArticles()
        {
            string cacheKey = "ApprovedArticles";
            if (!_cache.TryGetValue(cacheKey, out List<ArticleDto> articles))
            {
                articles = await _articleService.GetApprovedArticlesAsync();
                _cache.Set(cacheKey, articles, TimeSpan.FromMinutes(30));
            }
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle(int id)
        {
            var article = await _articleRepository.GetByIdAsync(id);
            if (article == null || article.Status != "Approved")
                return NotFound();
            return Ok(_mapper.Map<ArticleDto>(article));
        }

        [HttpGet("reports/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReport(int id)
        {
            var report = await _articleRepository.GetReportByIdAsync(id);
            if (report == null)
                return NotFound();
            return Ok(_mapper.Map<ReportDto>(report));
        }

        [HttpGet("users/{id}/reports")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserReports(int id)
        {
            var userReports = await _articleService.GetUserReportsAsync(id);
            return Ok(userReports);
        }
    }

    public class ResolveReportDto
    {
        public bool IsViolation { get; set; }
        public string Reason { get; set; }
    }
}   