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
    }
}
