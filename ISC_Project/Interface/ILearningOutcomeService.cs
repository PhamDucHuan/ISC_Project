using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface ILearningOutcomeService
    {
        // Read
        Task<IEnumerable<LearningOutcome>> GetAllAsync();
        Task<LearningOutcome?> GetByIdAsync(int id);
        Task<IEnumerable<LearningOutcome>> GetByUserIdAsync(int userId);

        // Create
        Task<LearningOutcome> CreateAsync(LearningOutcome newLearningOutcome);
    }
}
