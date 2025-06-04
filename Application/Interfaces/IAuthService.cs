using System.Threading.Tasks;
using FengShuiWeb.Application.DTOs;

namespace FengShuiWeb.Application.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(RegisterDto model);
        Task<AuthResponseDto> SignInAsync(SignInDto model);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task<string> GenerateResetPasswordTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<bool> ConfirmEmailAsync(string token);
        Task<AuthResponseDto> HandleGoogleLoginAsync(string email, string name);
        Task<AuthResponseDto> HandleFacebookLoginAsync(string email, string name);
    }
}