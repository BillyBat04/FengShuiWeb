using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FengShuiWeb.Application.DTOs
{
    public class FengShuiRequestDto
    {
        [Required]
        public int BirthYear { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string MainDoorDirection { get; set; }
    }

    public class FengShuiResponseDto
    {
        public FengShuiProfileDto Profile { get; set; }
        public DirectionInfoDto[] Directions { get; set; }
        public LayoutTipDto[] LayoutTips { get; set; }
        public string[] FriendlyExplanation { get; set; }
        public RecommendationDto Recommendations { get; set; }
    }

    public class FengShuiProfileDto
    {
        public string BirthYear { get; set; }
        public string DestinyElement { get; set; }
        public int KuaNumber { get; set; }
        public string KuaGroup { get; set; }
    }

    public class DirectionInfoDto
    {
        public string Direction { get; set; }
        public string Meaning { get; set; }
        public bool IsAuspicious { get; set; }
    }

    public class LayoutTipDto
    {
        public string Section { get; set; }
        public string Tip { get; set; }
    }

    public class FengShuiAnalysisDto
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string AnalysisData { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ComparisonRequestDto
    {
        public List<int> AnalysisIds { get; set; }
    }

    public class ComparisonResponseDto
    {
        public List<FengShuiAnalysisDto> Analyses { get; set; }
        public string Advice { get; set; }
    }

    public class RecommendationDto
    {
        public List<string> Articles { get; set; }
        public string SearchKeywords { get; set; }
    }
}