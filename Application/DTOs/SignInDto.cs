﻿using System.ComponentModel.DataAnnotations;

namespace FengShuiWeb.Application.DTOs  
{
    public class SignInDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}