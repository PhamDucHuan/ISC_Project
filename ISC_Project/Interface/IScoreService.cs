using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IScoreService
    {
        Task<IEnumerable<Score>> GetAllAsync();
        Task<Score> CreateAsync(Score score);
    }
}
