using FengShuiWeb.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FengShuiWeb.Application.Interfaces
{
    /// <summary>
    /// Interface định nghĩa các phương thức truy cập dữ liệu người dùng.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Lấy người dùng theo ID.
        /// </summary>
        /// <param name="id">ID của người dùng.</param>
        /// <returns>Người dùng hoặc null nếu không tìm thấy.</returns>
        Task<User> GetByIdAsync(int id);

        /// <summary>
        /// Lấy người dùng theo email.
        /// </summary>
        /// <param name="email">Email của người dùng.</param>
        /// <returns>Người dùng hoặc null nếu không tìm thấy.</returns>
        Task<User> GetByEmailAsync(string email);

        /// <summary>
        /// Lấy người dùng theo token và loại token.
        /// </summary>
        /// <param name="token">Giá trị token.</param>
        /// <param name="type">Loại token (Refresh, Reset, Confirmation).</param>
        /// <returns>Người dùng hoặc null nếu không tìm thấy.</returns>
        Task<User> GetByTokenAsync(string token, TokenType type);

        /// <summary>
        /// Lấy danh sách tất cả người dùng.
        /// </summary>
        /// <returns>Danh sách người dùng.</returns>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Tạo người dùng mới.
        /// </summary>
        /// <param name="user">Thông tin người dùng.</param>
        /// <returns>Task hoàn thành khi tạo thành công.</returns>
        Task CreateAsync(User user);

        /// <summary>
        /// Cập nhật thông tin người dùng.
        /// </summary>
        /// <param name="user">Thông tin người dùng cần cập nhật.</param>
        /// <returns>Task hoàn thành khi cập nhật thành công.</returns>
        Task UpdateAsync(User user);

        /// <summary>
        /// Xóa người dùng theo ID.
        /// </summary>
        /// <param name="id">ID của người dùng.</param>
        /// <returns>Task hoàn thành khi xóa thành công.</returns>
        Task DeleteAsync(int id);
    }
}