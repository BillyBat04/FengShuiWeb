using System.ComponentModel.DataAnnotations;

namespace FengShuiWeb.Application.DTOs
{
    public class ConfirmEmailDto
    {
        [Required]
        public string Token { get; set; }
    }
}