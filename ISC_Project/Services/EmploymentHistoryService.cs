using ISC_Project.DTOs.EmploymentHistory;
using ISC_Project.Interface;
using ISC_Project.Models;
using ISC_Project.Data;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class EmploymentHistoryService : IEmploymentHistoryService
    {
        private readonly ISC_ProjectDbContext _context;
        public EmploymentHistoryService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmploymentHistoryDto>> GetAllAsync()
        {
            return await _context.EmploymentHistories.Select(e => new EmploymentHistoryDto
            {
                HistoryId = e.HistoryId,
                Status = e.Status,
                EffectiveDate = e.EffectiveDate,
                Note = e.Note,
                Certificate = e.Certificate,
                Form = e.Form,
                DecidedRetireUrl = e.DecidedRetireUrl,
                CreatedById = e.CreatedById,
                UserId = e.UserId
            }).ToListAsync();
        }

        public async Task<EmploymentHistoryDto?> GetByIdAsync(int id)
        {
            var e = await _context.EmploymentHistories.FindAsync(id);
            if (e == null) return null;
            return new EmploymentHistoryDto
            {
                HistoryId = e.HistoryId,
                Status = e.Status,
                EffectiveDate = e.EffectiveDate,
                Note = e.Note,
                Certificate = e.Certificate,
                Form = e.Form,
                DecidedRetireUrl = e.DecidedRetireUrl,
                CreatedById = e.CreatedById,
                UserId = e.UserId
            };
        }

        public async Task<EmploymentHistoryDto> CreateAsync(CreateEmploymentHistoryDto dto)
        {
            var entity = new EmploymentHistory
            {
                Status = dto.Status,
                EffectiveDate = dto.EffectiveDate,
                Note = dto.Note,
                Certificate = dto.Certificate,
                Form = dto.Form,
                DecidedRetireUrl = dto.DecidedRetireUrl,
                CreatedById = dto.CreatedById,
                UserId = dto.UserId
            };
            _context.EmploymentHistories.Add(entity);
            await _context.SaveChangesAsync();
            return new EmploymentHistoryDto
            {
                HistoryId = entity.HistoryId,
                Status = entity.Status,
                EffectiveDate = entity.EffectiveDate,
                Note = entity.Note,
                Certificate = entity.Certificate,
                Form = entity.Form,
                DecidedRetireUrl = entity.DecidedRetireUrl,
                CreatedById = entity.CreatedById,
                UserId = entity.UserId
            };
        }
    }
} 