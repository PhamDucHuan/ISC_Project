using ISC_Project.Interface;
using ISC_Project.Models;

namespace Thuc_Tap_ISC.Services
{
    public class LearningOutcomeService : ILearningOutcomeService
    {
        private readonly ILearningOutcomeService _context;

        public LearningOutcomeService(ILearningOutcomeService context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LearningOutcome>> GetAllAsync()
        {
            return await _context.GetAllAsync();
        }

        public async Task<LearningOutcome?> GetByIdAsync(int id)
        {
            return await _context.GetByIdAsync(id);
        }

        public async Task<LearningOutcome> AddAsync(LearningOutcome entity)
        {
            return await _context.AddAsync(entity);
        }

        public async Task UpdateAsync(LearningOutcome entity)
        {
            await _context.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.DeleteAsync(id);
        }
    }
}
