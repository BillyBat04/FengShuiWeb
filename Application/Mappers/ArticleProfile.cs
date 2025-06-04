using AutoMapper;
using FengShuiWeb.Domain.Models;
using FengShuiWeb.Application.DTOs;

namespace FengShuiWeb.Application.Mappers
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<CreateArticleDto, Article>();
            CreateMap<UpdateArticleDto, Article>();
            CreateMap<Article, ArticleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name));
            CreateMap<ArticleHistory, ArticleHistoryDto>();
            CreateMap<CreateReportDto, Report>();
            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article.Title))
                .ForMember(dest => dest.ReporterName, opt => opt.MapFrom(src => src.Reporter != null ? src.Reporter.Name : null));
            CreateMap<Warning, WarningDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : null));
            CreateMap<User, UserReportDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Reports, opt => opt.MapFrom(src => src.Articles.SelectMany(a => a.Reports)));
        }
    }
}