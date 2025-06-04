using FengShuiWeb.Application.DTOs;
using System;

namespace FengShuiWeb.Application.DTOs
{
    public class AuthResponseDto
    {
        public UserDto User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}