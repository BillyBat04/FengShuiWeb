namespace FengShuiWeb.Models
{
    public class ZodiacRecommendation
    {
        public int Id { get; set; }
        public string ZodiacSign { get; set; } = string.Empty;
        public string Recommendation {  get; set; } = string.Empty;
    }
}
