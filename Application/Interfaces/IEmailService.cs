using System.Threading.Tasks;

namespace FengShuiWeb.Application.Interfaces
{
    /// <summary>
    /// Interface định nghĩa các phương thức gửi email.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Gửi email xác nhận tài khoản.
        /// </summary>
        /// <param name="to">Địa chỉ email người nhận.</param>
        /// <param name="token">Token xác nhận email.</param>
        /// <returns>Task hoàn thành khi gửi email thành công.</returns>
        Task SendEmailConfirmationAsync(string to, string token);

        /// <summary>
        /// Gửi email đặt lại mật khẩu.
        /// </summary>
        /// <param name="to">Địa chỉ email người nhận.</param>
        /// <param name="token">Token đặt lại mật khẩu.</param>
        /// <returns>Task hoàn thành khi gửi email thành công.</returns>
        Task SendResetPasswordEmailAsync(string to, string token);
        Task SendEmailAsync(string to, string subject, string body);
    }
}   