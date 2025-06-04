using System.ComponentModel.DataAnnotations;

namespace FengShuiWeb.Application.DTOs
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}