using ISC_Project.Data;
using ISC_Project.DTOs.Enrollment;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ISC_ProjectDbContext _context;
        public EnrollmentService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto)
        {
            // Lỗi 1: Kiểm tra sự tồn tại của User và Class
            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
            {
                throw new KeyNotFoundException($"Không tìm thấy người dùng (học sinh) với ID {dto.UserId}.");
            }

            var classExists = await _context.Classes.AnyAsync(c => c.ClassId == dto.ClassId);
            if (!classExists)
            {
                throw new KeyNotFoundException($"Không tìm thấy lớp học với ID {dto.ClassId}.");
            }

            // Lỗi 2: Kiểm tra xem học sinh đã được ghi danh vào lớp này trong năm học này chưa
            var alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.UserId == dto.UserId && e.ClassId == dto.ClassId && e.SchoolYearId == dto.SchoolYearId);
            if (alreadyEnrolled)
            {
                throw new ArgumentException($"Học sinh đã được ghi danh vào lớp này trong năm học hiện tại.");
            }

            var enrollment = new Enrollment
            {
                Status = dto.Status,
                UserId = dto.UserId,
                ClassId = dto.ClassId,
                SchoolYearId = dto.SchoolYearId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return new EnrollmentDto
            {
                EnrollmentsId = enrollment.EnrollmentsId,
                Status = enrollment.Status,
                UserId = enrollment.UserId,
                ClassId = enrollment.ClassId
            };
        }

        public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Select(e => new EnrollmentDto
                {
                    EnrollmentsId = e.EnrollmentsId,
                    Status = e.Status,
                    UserId = e.UserId,
                    ClassId = e.ClassId
                }).ToListAsync();
        }

        public async Task<EnrollmentDto?> GetByIdAsync(int id)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Where(e => e.EnrollmentsId == id)
                .Select(e => new EnrollmentDto
                {
                    EnrollmentsId = e.EnrollmentsId,
                    Status = e.Status,
                    UserId = e.UserId,
                    ClassId = e.ClassId
                }).FirstOrDefaultAsync();
        }
    }
}