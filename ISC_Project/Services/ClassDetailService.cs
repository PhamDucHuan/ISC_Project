using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Services
{
    public class ClassDetailService : IClassDetailService
    {
        private readonly ISC_ProjectDbContext _context;

        public ClassDetailService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassDetail>> GetAllAsync()
        {
            return await _context.ClassDetails
                .Include(c => c.Class)
                .Include(c => c.Department)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<ClassDetail?> GetByIdAsync(int id)
        {
            return await _context.ClassDetails
                .Include(c => c.Class)
                .Include(c => c.Department)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.DetailClassId == id);
        }

        public async Task<ClassDetail> CreateAsync(ClassDetail detail)
        {
            _context.ClassDetails.Add(detail);
            await _context.SaveChangesAsync();
            return detail;
        }

        public async Task<bool> UpdateAsync(int id, ClassDetail detail)
        {
            var existing = await _context.ClassDetails.FindAsync(id);
            if (existing == null) return false;

            existing.AdmissionDate = detail.AdmissionDate;
            existing.Status = detail.Status;
            existing.NumberOfSubjects = detail.NumberOfSubjects;
            existing.Description = detail.Description;
            existing.StudentId = detail.StudentId;
            existing.ClassId = detail.ClassId;
            existing.DepartmentId = detail.DepartmentId;
            existing.UserId = detail.UserId;
            existing.SchoolYearId = detail.SchoolYearId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.ClassDetails.FindAsync(id);
            if (existing == null) return false;

            _context.ClassDetails.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
