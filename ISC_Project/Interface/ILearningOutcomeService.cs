using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface ILearningOutcomeService
    {
        Task<IEnumerable<LearningOutcome>> GetAllAsync();
        Task<LearningOutcome?> GetByIdAsync(int id);
        Task<LearningOutcome> AddAsync(LearningOutcome entity);
    }
}
