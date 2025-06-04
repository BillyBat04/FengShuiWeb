using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Infrastructure.Data;
using FengShuiWeb.Application.Interfaces;

namespace FengShuiWeb.Infrastructure
{
    public class TokenRepository : ITokenRepository
    {
        private readonly FengShuiDbContext _context;

        public TokenRepository(FengShuiDbContext context)
        {
            _context = context;
        }

        public async Task<Token> GetByValueAsync(string tokenValue, TokenType type)
        {
            return await _context.Tokens
                .FirstOrDefaultAsync(t => t.TokenValue == tokenValue && t.Type == type);
        }

        public async Task CreateAsync(Token token)
        {
            await _context.Tokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeAsync(string tokenValue, TokenType type)
        {
            var token = await _context.Tokens
                .FirstOrDefaultAsync(t => t.TokenValue == tokenValue && t.Type == type);
            if (token != null)
            {
                token.IsRevoked = true;
                _context.Tokens.Update(token);
                await _context.SaveChangesAsync();
            }
        }
    }
}