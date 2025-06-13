using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Serilog;
using FengShuiWeb.Infrastructure.Data;
using FengShuiWeb.Application.Interfaces;
using FengShuiWeb.Application.Services;
using FengShuiWeb.Infrastructure.Repositories;
using AutoMapper;
using FengShuiWeb.Application.Mappers;
using FengShuiWeb.Application;
using FengShuiWeb.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Identity;
using FengShuiWeb.Infrastructure.DataProviders;
using Microsoft.AspNetCore.Authentication.Cookies;

Env.Load(); 
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
    config.WriteTo.Console();
    config.WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day);
});


builder.Services.AddDbContext<FengShuiDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(AuthProfile), typeof(FengShuiProfile), typeof(ArticleProfile));

builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Thêm DefaultSignInScheme
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.CallbackPath = "/signin-google"; 
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    options.Fields.Add("email");
    options.Fields.Add("name");
});


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFengShuiService, FengShuiService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IFengShuiAnalysisRepository, FengShuiAnalysisRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IPasswordHasher<FengShuiWeb.Domain.Models.User>, PasswordHasher<FengShuiWeb.Domain.Models.User>>();
builder.Services.AddScoped<FengShuiDataProvider>();
builder.Services.AddSingleton<JwtService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "FengShuiWeb API",
        Version = "v1",
        Description = "API for FengShuiWeb application",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Your Name",
            Email = "your-email@example.com"
        }
    });
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0; 
});
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();