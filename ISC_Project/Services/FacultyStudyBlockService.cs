using ISC_Project.Data;
using ISC_Project.DTOs.FacultyStudyBlock;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class FacultyStudyBlockService : IFacultyStudyBlockService
    {
        private readonly ISC_ProjectDbContext _context;

        public FacultyStudyBlockService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IFacultyStudyBlockService>> GetAllFacultyStudyBlocksAsync()
        {
            return (IEnumerable<IFacultyStudyBlockService>)await _context.FacultyStudyBlocks.ToListAsync();
        }

        public async Task<FacultyStudyBlock?> GetFacultyStudyBlockByIdAsync(int id)
        {
            return await _context.FacultyStudyBlocks.FindAsync(id);
        }

        public async Task<FacultyStudyBlock> CreateFacultyStudyBlockAsync(CreateFacultyDto dto)
        {
            var entity = new FacultyStudyBlock { FacultyName = dto.FacultyName, FacultyCode = dto.FacultyCode };
            _context.FacultyStudyBlocks.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
