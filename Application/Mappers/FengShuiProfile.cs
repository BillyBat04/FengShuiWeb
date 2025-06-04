using AutoMapper;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Application.DTOs;

namespace FengShuiWeb.Application.Mappers
{
    public class FengShuiProfile : Profile
    {
        public FengShuiProfile()
        {
            CreateMap<FengShuiAnalysis, FengShuiAnalysisDto>();
        }
    }
}   