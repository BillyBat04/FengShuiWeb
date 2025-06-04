using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Infrastructure;
using FengShuiWeb.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using FengShuiWeb.Application.Interfaces;

namespace FengShuiWeb.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new ArgumentException("Người dùng không tồn tại");
            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new ArgumentException("Người dùng không tồn tại");

            user.Name = dto.Name;
            user.BirthDate = dto.BirthDate;
            user.Gender = dto.Gender;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new ArgumentException("Người dùng không tồn tại");

            await _userRepository.DeleteAsync(id);
        }

        public async Task<UserDto> GetOwnProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("Người dùng không tồn tại");
            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateOwnProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("Người dùng không tồn tại");

            if (!string.IsNullOrEmpty(dto.Name))
                user.Name = dto.Name;
            if (dto.BirthDate.HasValue)
                user.BirthDate = dto.BirthDate.Value;
            if (!string.IsNullOrEmpty(dto.Gender))
                user.Gender = dto.Gender;

            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangeOwnPasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("Người dùng không tồn tại");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                throw new AuthenticationException("Mật khẩu hiện tại không đúng");

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
            await _userRepository.UpdateAsync(user);
        }
    }
}