using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Application.Interfaces;
using FengShuiWeb.Infrastructure.Data;

namespace FengShuiWeb.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly FengShuiDbContext _context;

        public UserRepository(FengShuiDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email không được để trống");

            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByTokenAsync(string token, TokenType type)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token không được để trống");

            var tokenEntity = await _context.Tokens
                .FirstOrDefaultAsync(t => t.TokenValue == token && t.Type == type && !t.IsRevoked && t.ExpiryDate > DateTime.UtcNow);
            return tokenEntity != null ? await _context.Users.FindAsync(tokenEntity.UserID) : null;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task CreateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new ArgumentException("Người dùng không tồn tại");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}