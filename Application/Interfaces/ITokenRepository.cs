using FengShuiWeb.Domain.Models;
using System.Threading.Tasks;

namespace FengShuiWeb.Application.Interfaces
{
    /// <summary>
    /// Interface định nghĩa các phương thức quản lý token.
    /// </summary>
    public interface ITokenRepository
    {
        /// <summary>
        /// Lấy token theo giá trị và loại.
        /// </summary>
        /// <param name="tokenValue">Giá trị token.</param>
        /// <param name="type">Loại token (Refresh, Reset, Confirmation).</param>
        /// <returns>Token hoặc null nếu không tìm thấy.</returns>
        Task<Token> GetByValueAsync(string tokenValue, TokenType type);

        /// <summary>
        /// Tạo token mới.
        /// </summary>
        /// <param name="token">Thông tin token.</param>
        /// <returns>Task hoàn thành khi tạo thành công.</returns>
        Task CreateAsync(Token token);

        /// <summary>
        /// Hủy token theo giá trị và loại.
        /// </summary>
        /// <param name="tokenValue">Giá trị token.</param>
        /// <param name="type">Loại token.</param>
        /// <returns>Task hoàn thành khi hủy thành công.</returns>
        Task RevokeAsync(string tokenValue, TokenType type);
    }
}