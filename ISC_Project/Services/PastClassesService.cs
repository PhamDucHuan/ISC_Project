using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class PastClassesService : IPastClassesService
    {
        private readonly ISC_ProjectDbContext _context;

        public PastClassesService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PastClass>> GetAllAsync()
        {
            // Dùng Include để tải thông tin User và Subject liên quan
            return await _context.PastClasses
                                 .Include(c => c.User)
                                 .Include(c => c.Subject)
                                 .ToListAsync();
        }

        public async Task<PastClass?> GetByIdAsync(int classId)
        {
            // Dùng Include để tải thông tin User và Subject liên quan
            return await _context.PastClasses
                                 .Include(c => c.User)
                                 .Include(c => c.Subject)
                                 .FirstOrDefaultAsync(c => c.ClassId == classId);
        }

        public async Task<PastClass> CreateAsync(PastClass newClass)
        {
            // Gán thời gian bắt đầu nếu chưa có
            if (newClass.StartTime == null)
            {
                newClass.StartTime = DateTime.UtcNow;
            }

            _context.PastClasses.Add(newClass);
            await _context.SaveChangesAsync();
            return newClass;
        }
    }
}
