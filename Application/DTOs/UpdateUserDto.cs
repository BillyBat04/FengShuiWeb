using System;
using System.ComponentModel.DataAnnotations;

namespace FengShuiWeb.Application.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}