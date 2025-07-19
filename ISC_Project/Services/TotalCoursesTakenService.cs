using ISC_Project.Data;
using ISC_Project.DTOs.TotalCoursesTaken;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class TotalCoursesTakenService : ITotalCoursesTakenService
    {
        private readonly ISC_ProjectDbContext _context;

        public TotalCoursesTakenService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<TotalCoursesTakenDto> CreateAsync(CreateTotalCoursesTakenDto dto)
        {
            // Lỗi 1: Kiểm tra sự tồn tại của User
            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
            {
                throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {dto.UserId}.");
            }

            // Lỗi 2: Kiểm tra xem người dùng này đã có bản ghi TotalCoursesTaken chưa
            var recordExists = await _context.TotalCoursesTakens.AnyAsync(t => t.UserId == dto.UserId);
            if (recordExists)
            {
                throw new ArgumentException($"Người dùng với ID {dto.UserId} đã có bản ghi tổng hợp khóa học.");
            }

            var totalCoursesTaken = new TotalCoursesTaken
            {
                UserId = dto.UserId,
                TotalNumberCourses = dto.TotalNumberCourses ?? 0,
                TotalPayment = dto.TotalPayment ?? 0,
                CoursesLearnedId = dto.CoursesLearnedId
            };

            _context.TotalCoursesTakens.Add(totalCoursesTaken);
            await _context.SaveChangesAsync();

            return new TotalCoursesTakenDto
            {
                TotalCoursesId = totalCoursesTaken.TotalCoursesId,
                TotalNumberCourses = totalCoursesTaken.TotalNumberCourses,
                TotalPayment = totalCoursesTaken.TotalPayment,
                UserId = totalCoursesTaken.UserId
            };
        }

        public async Task<IEnumerable<TotalCoursesTakenDto>> GetAllAsync()
        {
            return await _context.TotalCoursesTakens.AsNoTracking().Select(t => new TotalCoursesTakenDto { TotalCoursesId = t.TotalCoursesId, TotalNumberCourses = t.TotalNumberCourses, TotalPayment = t.TotalPayment, UserId = t.UserId }).ToListAsync();
        }

        public async Task<TotalCoursesTakenDto?> GetByIdAsync(int id)
        {
            return await _context.TotalCoursesTakens.AsNoTracking().Where(t => t.TotalCoursesId == id).Select(t => new TotalCoursesTakenDto { TotalCoursesId = t.TotalCoursesId, TotalNumberCourses = t.TotalNumberCourses, TotalPayment = t.TotalPayment, UserId = t.UserId }).FirstOrDefaultAsync();
        }
    }
}