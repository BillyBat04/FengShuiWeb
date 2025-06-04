﻿using System.ComponentModel.DataAnnotations;

namespace FengShuiWeb.Application.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}