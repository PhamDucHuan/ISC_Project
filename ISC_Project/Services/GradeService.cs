using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ISC_Project.Services
{
    public class GradeService : IGradeService
    {
        private readonly ISC_ProjectDbContext _context;

        public GradeService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Grade>> GetAllGradesAsync()
        {
            return await _context.Grades
                .Include(g => g.User)
                .Include(g => g.Training)
                .Include(g => g.SchoolYear)
                .ToListAsync();
        }

        public async Task<Grade> GetGradeByIdAsync(int id)
        {
            return await _context.Grades
                .Include(g => g.User)
                .Include(g => g.Training)
                .Include(g => g.SchoolYear)
                .FirstOrDefaultAsync(g => g.GradeId == id);
        }

        public async Task<Grade> CreateGradeAsync(Grade grade)
        {
            grade.CreateAt = DateTime.UtcNow;
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return grade;
        }
    }
}
