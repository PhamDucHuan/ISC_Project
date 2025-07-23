
using ISC_Project.Models;
using ISC_Project.Data;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class QualificationService : IQualificationsService
    {
        private readonly ISC_ProjectDbContext _context;

        public QualificationService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Qualification>> GetAllAsync()
        {
            return await _context.Qualifications.ToListAsync();
        }

        public async Task<Qualification?> GetByIdAsync(int id)
        {
            return await _context.Qualifications.FindAsync(id);
        }

        public async Task<Qualification> CreateAsync(Qualification model)
        {
            _context.Qualifications.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task UpdateAsync(Qualification model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}

