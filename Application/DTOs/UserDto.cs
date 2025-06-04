using System;

namespace FengShuiWeb.Application.DTOs
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsActive { get; set; }
    }
}