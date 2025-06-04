using System;
using System.ComponentModel.DataAnnotations;

namespace FengShuiWeb.Domain.Models
{
    public enum TokenType
    {
        RefreshToken,
        ResetPassword,
        EmailConfirmation
    }

    public class Token
    {
        [Key]
        public int TokenID { get; set; }
        [Required]
        public string TokenValue { get; set; }
        public int UserID { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; } = false;
        public TokenType Type { get; set; }
        public User User { get; set; }
    }
}