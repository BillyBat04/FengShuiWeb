using AutoMapper;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Application.DTOs;

namespace FengShuiWeb.Application.Mappers
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"))
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<User, UserDto>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<UpdateProfileDto, User>();
        }
    }
}