using FengShuiWeb.Models;

namespace FengShuiWeb.Repositories
{
    public interface IZodiacRepository
    {
        ZodiacRecommendation? GetRecommendation(string zodiac);
    }
}
