using FengShuiWeb.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FengShuiWeb.Application.Interfaces
{
    public interface IFengShuiAnalysisRepository
    {
        /// <summary>
        /// Lưu một bài phân tích phong thủy vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="analysis">Bài phân tích cần lưu.</param>
        Task CreateAsync(FengShuiAnalysis analysis);

        /// <summary>
        /// Lấy danh sách các bài phân tích của một người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <returns>Danh sách các bài phân tích.</returns>
        Task<List<FengShuiAnalysis>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Lấy các bài phân tích theo danh sách ID và người dùng.
        /// </summary>
        /// <param name="ids">Danh sách ID bài phân tích.</param>
        /// <param name="userId">ID của người dùng.</param>
        /// <returns>Danh sách các bài phân tích phù hợp.</returns>
        Task<List<FengShuiAnalysis>> GetByIdsAsync(List<int> ids, int userId);
    }
}