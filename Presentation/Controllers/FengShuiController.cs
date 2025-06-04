using FengShuiWeb.Application.DTOs;
using FengShuiWeb.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FengShuiWeb.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FengShuiController : ControllerBase
    {
        private readonly IFengShuiService _fengShuiService;

        public FengShuiController(IFengShuiService fengShuiService)
        {
            _fengShuiService = fengShuiService;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeFengShui([FromBody] FengShuiRequestDto dto)
        {
            var result = await _fengShuiService.GetFengShuiAnalysisAsync(dto);
            return Ok(result);
        }

        [HttpPost("save")]
        [Authorize]
        public async Task<IActionResult> SaveAnalysis([FromBody] FengShuiAnalysisDto analysis)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var analysisId = await _fengShuiService.SaveAnalysisAsync(userId, analysis);
            return Ok(new { Message = "Đã lưu bài phân tích thành công", AnalysisId = analysisId });
        }

        [HttpGet("saved")]
        [Authorize]
        public async Task<IActionResult> GetSavedAnalyses()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var analyses = await _fengShuiService.GetSavedAnalysesAsync(userId);
            return Ok(analyses);
        }

        [HttpPost("compare")]
        [Authorize]
        public async Task<IActionResult> CompareAnalyses([FromBody] ComparisonRequestDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var comparison = await _fengShuiService.CompareAnalysesAsync(userId, request);
            return Ok(comparison);
        }
    }
}