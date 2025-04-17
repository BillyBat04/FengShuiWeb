using FengShuiWeb.Data;
using FengShuiWeb.Models;
using FengShuiWeb.Repositories;

public class ZodiacRepository : IZodiacRepository
{
    private readonly DataContext _context;
    public ZodiacRepository(DataContext context)
    {
        _context = context;
    }

    public ZodiacRecommendation? GetRecommendation(string zodiac)
    {
        return _context.ZodiacRecommendations.FirstOrDefault(z => z.ZodiacSign.ToLower() == zodiac.ToLower());
    }
}