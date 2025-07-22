using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Services
{
    public class StudentsChangeClassService : IStudentsChangeClassService
    {
        private readonly ISC_ProjectDbContext _context;

        public StudentsChangeClassService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentsChangeClass>> GetAllAsync()
        {
            return await _context.StudentsChangeClasses
                                 .Include(s => s.User)
                                 .ToListAsync();
        }

        public async Task<StudentsChangeClass> AddAsync(StudentsChangeClass changeRequest)
        {
            _context.StudentsChangeClasses.Add(changeRequest);
            await _context.SaveChangesAsync();
            return changeRequest;
        }
        public async Task<StudentsChangeClass?> GetByIdAsync(int id)
        {
            return await _context.StudentsChangeClasses
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentsChangeClassesId == id);
        }

    }
}
