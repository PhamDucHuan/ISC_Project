using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class LearningOutcomeService : ILearningOutcomeService
    {
        private readonly ISC_ProjectDbContext _context;

        public LearningOutcomeService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<LearningOutcome> CreateAsync(LearningOutcome newLearningOutcome)
        {
            newLearningOutcome.UpdateAt = DateTime.UtcNow; // Tự động cập nhật thời gian
            _context.LearningOutcomes.Add(newLearningOutcome);
            await _context.SaveChangesAsync();
            return newLearningOutcome;
        }

        // READ
        public async Task<IEnumerable<LearningOutcome>> GetAllAsync()
        {
            return await _context.LearningOutcomes.ToListAsync();
        }

        public async Task<LearningOutcome?> GetByIdAsync(int id)
        {
            return await _context.LearningOutcomes.FindAsync(id);
        }

        public async Task<IEnumerable<LearningOutcome>> GetByUserIdAsync(int userId)
        {
            return await _context.LearningOutcomes
                                 .Where(lo => lo.UserId == userId)
                                 .ToListAsync();
        }
    }
}
