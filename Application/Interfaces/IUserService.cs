using FengShuiWeb.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FengShuiWeb.Application.Interfaces
{

    public interface IUserService
    {
        
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

     
        Task<UserDto> GetUserByIdAsync(int id);

    
        Task UpdateUserAsync(int id, UpdateUserDto dto);

        Task DeleteUserAsync(int id);

 
        Task<UserDto> GetOwnProfileAsync(int userId);

      
        Task UpdateOwnProfileAsync(int userId, UpdateProfileDto dto);


        Task ChangeOwnPasswordAsync(int userId, ChangePasswordDto dto);
    }
}