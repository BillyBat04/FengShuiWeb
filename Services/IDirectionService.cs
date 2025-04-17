using FengShuiWeb.DTOs;

namespace FengShuiWeb.Services
{
    public interface IDirectionService
    {
        LuckyDirectionResultDTO CalculateLuckyDirection(string gender, DateTime birthDate);
    }
}
