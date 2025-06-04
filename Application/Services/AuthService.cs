using System;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Infrastructure;
using FengShuiWeb.Application.DTOs;
using FengShuiWeb.Application.Interfaces;

namespace FengShuiWeb.Application
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepository,
            ITokenRepository tokenRepository,
            IPasswordHasher<User> passwordHasher,
            IOptions<JwtSettings> jwtSettings,
            IEmailService emailService,
            IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> RegisterUserAsync(RegisterDto model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrEmpty(model.Email)) throw new ArgumentException("Email không được để trống");
            if (string.IsNullOrEmpty(model.Password)) throw new ArgumentException("Mật khẩu không được để trống");

            if (!IsValidEmail(model.Email))
                throw new ArgumentException("Định dạng email không hợp lệ");

            var existingUser = await _userRepository.GetByEmailAsync(model.Email);
            if (existingUser != null)
                throw new ArgumentException("Email đã tồn tại");

            var user = _mapper.Map<User>(model);
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            await _userRepository.CreateAsync(user);

            var confirmationToken = GenerateEmailConfirmationToken();
            await SaveEmailConfirmationTokenAsync(confirmationToken, user);
            await _emailService.SendEmailConfirmationAsync(user.Email, confirmationToken);

            return true;
        }

        public async Task<AuthResponseDto> SignInAsync(SignInDto model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrEmpty(model.Email)) throw new ArgumentException("Email không được để trống");
            if (string.IsNullOrEmpty(model.Password)) throw new ArgumentException("Mật khẩu không được để trống");

            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null || !user.IsActive)
                throw new AuthenticationException("Email hoặc mật khẩu không hợp lệ");

            if (!user.IsEmailConfirmed)
                throw new AuthenticationException("Email chưa được xác nhận");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                throw new AuthenticationException("Email hoặc mật khẩu không hợp lệ");

            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(refreshToken, user);

            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentNullException(nameof(refreshToken));

            var existingToken = await _tokenRepository.GetByValueAsync(refreshToken, TokenType.RefreshToken);
            if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiryDate < DateTime.UtcNow)
                throw new AuthenticationException("Refresh token không hợp lệ hoặc đã hết hạn");

            var user = await _userRepository.GetByIdAsync(existingToken.UserID);
            if (user == null || !user.IsActive)
                throw new AuthenticationException("Người dùng không tồn tại hoặc không hoạt động");

            var newAccessToken = await GenerateJwtTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(newRefreshToken, user);
            await _tokenRepository.RevokeAsync(refreshToken, TokenType.RefreshToken);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentNullException(nameof(refreshToken));

            await _tokenRepository.RevokeAsync(refreshToken, TokenType.RefreshToken);
        }

        public async Task<string> GenerateResetPasswordTokenAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !user.IsActive)
                throw new ArgumentException("Người dùng không tồn tại hoặc không hoạt động");

            var resetToken = GenerateResetPasswordToken();
            await SaveResetPasswordTokenAsync(resetToken, user);
            return resetToken;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var existingToken = await _tokenRepository.GetByValueAsync(token, TokenType.ResetPassword);
            if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiryDate < DateTime.UtcNow)
                throw new ArgumentException("Token đặt lại mật khẩu không hợp lệ hoặc đã hết hạn");

            var user = await _userRepository.GetByIdAsync(existingToken.UserID);
            if (user == null)
                throw new ArgumentException("Người dùng không tồn tại");

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
            await _userRepository.UpdateAsync(user);
            await _tokenRepository.RevokeAsync(token, TokenType.ResetPassword);

            return true;
        }

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            var user = await _userRepository.GetByTokenAsync(token, TokenType.EmailConfirmation);
            if (user == null)
                throw new ArgumentException("Token xác nhận email không hợp lệ hoặc đã hết hạn");

            user.IsEmailConfirmed = true;
            await _userRepository.UpdateAsync(user);
            await _tokenRepository.RevokeAsync(token, TokenType.EmailConfirmation);

            return true;
        }

        public async Task<AuthResponseDto> HandleGoogleLoginAsync(string email, string name)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email không được để trống");

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Name = name,
                    Role = "User",
                    IsEmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    PasswordHash = _passwordHasher.HashPassword(null, Guid.NewGuid().ToString()) // Dummy password
                };
                await _userRepository.CreateAsync(user);
            }
            else if (!user.IsActive)
            {
                throw new AuthenticationException("Tài khoản không hoạt động");
            }

            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(refreshToken, user);

            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<AuthResponseDto> HandleFacebookLoginAsync(string email, string name)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email không được để trống");

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Name = name,
                    Role = "User",
                    IsEmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    PasswordHash = _passwordHasher.HashPassword(null, Guid.NewGuid().ToString()) // Dummy password
                };
                await _userRepository.CreateAsync(user);
            }
            else if (!user.IsActive)
            {
                throw new AuthenticationException("Tài khoản không hoạt động");
            }

            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();
            await SaveRefreshTokenAsync(refreshToken, user);

            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                User = _mapper.Map<UserDto>(user)
            };
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private async Task SaveRefreshTokenAsync(string refreshToken, User user)
        {
            var token = new Token
            {
                TokenValue = refreshToken,
                UserID = user.UserID,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
                Type = TokenType.RefreshToken
            };
            await _tokenRepository.CreateAsync(token);
        }

        private string GenerateResetPasswordToken()
        {
            return Guid.NewGuid().ToString();
        }

        private async Task SaveResetPasswordTokenAsync(string resetToken, User user)
        {
            var token = new Token
            {
                TokenValue = resetToken,
                UserID = user.UserID,
                ExpiryDate = DateTime.UtcNow.AddHours(1),
                Type = TokenType.ResetPassword
            };
            await _tokenRepository.CreateAsync(token);
        }

        private string GenerateEmailConfirmationToken()
        {
            return Guid.NewGuid().ToString();
        }

        private async Task SaveEmailConfirmationTokenAsync(string confirmationToken, User user)
        {
            var token = new Token
            {
                TokenValue = confirmationToken,
                UserID = user.UserID,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                Type = TokenType.EmailConfirmation
            };
            await _tokenRepository.CreateAsync(token);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}