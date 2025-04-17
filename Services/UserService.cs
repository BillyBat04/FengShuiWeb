using FengShuiWeb.DTOs;
using FengShuiWeb.Models;
using FengShuiWeb.Repositories;
using FengShuiWeb.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IDirectionService _directionService;

    public UserService(IUserRepository repo, IDirectionService directionService)
    {
        _repo = repo;
        _directionService = directionService;
    }

    public void RegisterUser(UserDTO dto)
    {
        var user = new User
        {
            Username = dto.Username,
            Gender = dto.Gender,
            BirthDate = dto.BirthDate,
            PasswordHash = "123" // Tạm thời bỏ qua mã hóa
        };

        _repo.AddUser(user);
    }

    public LuckyDirectionResultDTO GetLuckyDirection(string gender, DateTime birthDate)
    {
        return _directionService.CalculateLuckyDirection(gender, birthDate);
    }
}