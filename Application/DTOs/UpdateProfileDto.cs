using System;

namespace FengShuiWeb.Application.DTOs
{
    public class UpdateProfileDto
    {
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
    }
}