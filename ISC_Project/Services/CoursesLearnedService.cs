using ISC_Project.Data;
using ISC_Project.Interface;
 
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Services
{
    public class CoursesLearnedService : ICoursesLearnedService
    {
        private readonly ISC_ProjectDbContext _context;

        public CoursesLearnedService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CoursesLearned>> GetAllAsync()
        {
            return await _context.CoursesLearneds
                                 .Include(c => c.TotalCourses)
                                 .ToListAsync();
        }

        public async Task<CoursesLearned?> GetByIdAsync(int id)
        {
            return await _context.CoursesLearneds
                                 .Include(c => c.TotalCourses)
                                 .FirstOrDefaultAsync(c => c.CoursesLearnedId == id);
        }

        public async Task<CoursesLearned> CreateAsync(CoursesLearned course)
        {
            _context.CoursesLearneds.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }
    }
}
