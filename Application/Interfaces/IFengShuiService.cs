using FengShuiWeb.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FengShuiWeb.Application.Interfaces
{

    public interface IFengShuiService
    {
        Task<FengShuiResponseDto> GetFengShuiAnalysisAsync(FengShuiRequestDto dto);
        Task<int> SaveAnalysisAsync(int userId, FengShuiAnalysisDto analysis);

        Task<List<FengShuiAnalysisDto>> GetSavedAnalysesAsync(int userId);

        Task<ComparisonResponseDto> CompareAnalysesAsync(int userId, ComparisonRequestDto request);

        Task<RecommendationDto> GetRecommendationsAsync(string analysisData);
        Task<List<FengShuiAnalysisDto>> SearchAnalysesAsync(int userId, SearchAnalysisDto searchDto);
    }
}