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

        public async Task<bool> UpdateAsync(int id, CoursesLearned course)
        {
            var existing = await _context.CoursesLearneds.FindAsync(id);
            if (existing == null) return false;

            existing.Title = course.Title;
            existing.Description = course.Description;
            existing.Syllabus = course.Syllabus;
            existing.DefaultPrice = course.DefaultPrice;
            existing.CoursesImageUrl = course.CoursesImageUrl;
            existing.TotalCoursesId = course.TotalCoursesId;
            existing.SchoolYearId = course.SchoolYearId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.CoursesLearneds.FindAsync(id);
            if (existing == null) return false;

            _context.CoursesLearneds.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
