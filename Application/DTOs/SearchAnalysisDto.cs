using System;

namespace FengShuiWeb.Application.DTOs
{
    public class SearchAnalysisDto
    {
        public string Label { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string KeywordInAnalysis { get; set; }
    }
}