using ISC_Project.DTOs.Score;
using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IScoreService
    {
        Task<IEnumerable<Score>> GetAllAsync();
        Task<Score?> GetByIdAsync(int id); 
        Task<Score> CreateAsync(CreateScoreDto scoreDto);
    }
}
