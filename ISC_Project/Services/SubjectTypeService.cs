using ISC_Project.Data;
using ISC_Project.DTOs.SubjectType;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class SubjectTypeService : ISubjectTypeService
    {
        private readonly ISC_ProjectDbContext _context;

        public SubjectTypeService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<SubjectTypeDto?> GetByIdAsync(int subjectTypeId)
        {
            return await _context.SubjectTypes
                .AsNoTracking()
                .Where(s => s.SubjectTypeId == subjectTypeId)
                .Select(s => new SubjectTypeDto
                {
                    SubjectTypeId = s.SubjectTypeId,
                    SubjectTypeName = s.SubjectTypeName,
                    Description = s.Description,
                    Status = s.Status,
                    Note = s.Note
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SubjectTypeDto>> GetAllAsync()
        {
            return await _context.SubjectTypes
                .AsNoTracking()
                .Select(s => new SubjectTypeDto
                {
                    SubjectTypeId = s.SubjectTypeId,
                    SubjectTypeName = s.SubjectTypeName,
                    Description = s.Description,
                    Status = s.Status,
                    Note = s.Note
                })
                .ToListAsync();
        }

        public async Task<SubjectType> CreateAsync(CreateSubjectTypeDto dto)
        {
            var newSubjectType = new SubjectType
            {
                SubjectTypeName = dto.SubjectTypeName,
                Description = dto.Description,
                Status = dto.Status,
                Note = dto.Note
            };

            _context.SubjectTypes.Add(newSubjectType);
            await _context.SaveChangesAsync();
            return newSubjectType;
        }
    }
}