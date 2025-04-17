using FengShuiWeb.DTOs;

namespace FengShuiWeb.Services
{
    public interface IUserService
    {
        void RegisterUser(UserDTO dto);
        LuckyDirectionResultDTO GetLuckyDirection(string gender, DateTime birthDate);
    }
}
