using FengShuiWeb.Application.Interfaces;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FengShuiWeb.Infrastructure.Repositories
{
    public class FengShuiAnalysisRepository : IFengShuiAnalysisRepository
    {
        private readonly FengShuiDbContext _context;

        public FengShuiAnalysisRepository(FengShuiDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(FengShuiAnalysis analysis)
        {
            await _context.FengShuiAnalyses.AddAsync(analysis);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FengShuiAnalysis>> GetByUserIdAsync(int userId)
        {
            return await _context.FengShuiAnalyses
                .Where(a => a.UserID == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<FengShuiAnalysis>> GetByIdsAsync(List<int> ids, int userId)
        {
            return await _context.FengShuiAnalyses
                .Where(a => a.UserID == userId && ids.Contains(a.Id))
                .ToListAsync();
        }
    }
}
