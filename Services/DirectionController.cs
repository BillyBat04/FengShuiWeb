using FengShuiWeb.DTOs;
using FengShuiWeb.Helpers;
using FengShuiWeb.Services;

public class DirectionService : IDirectionService
{
    public LuckyDirectionResultDTO CalculateLuckyDirection(string gender, DateTime birthDate)
    {
        var year = birthDate.Year;
        var direction = LuckyDirectionHelper.GetDirection(gender, year);

        return new LuckyDirectionResultDTO
        {
            Direction = direction,
            Explanation = $"Dựa theo năm sinh {year} và giới tính {gender}, hướng may mắn là {direction}."
        };
    }
}