using ISC_Project.Interface;
using ISC_Project.Models;

namespace ISC_Project.Services
{
    public class LabGraderService : ILabGraderService
    {
        private readonly ILabGraderService _context;

        public LabGraderService(ILabGraderService context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LabGrader>> GetAllAsync()
        {
            return await _context.GetAllAsync();
        }

        public async Task<LabGrader?> GetByKeyAsync(int labId, int userId)
        {
            return await _context.GetByKeyAsync(labId, userId);
        }

        public async Task<LabGrader> AddAsync(LabGrader entity)
        {
            return await _context.AddAsync(entity);
        }
    }
}
