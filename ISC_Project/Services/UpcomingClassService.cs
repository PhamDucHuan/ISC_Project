using ISC_Project.Data;
using ISC_Project.DTOs.UpcomingClass;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class UpcomingClassService : IUpcomingClassService
    {
        private readonly ISC_ProjectDbContext _context;

        public UpcomingClassService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<UpcomingClassDto> CreateAsync(CreateUpcomingClassDto dto)
        {
            // Lỗi 1: Kiểm tra sự tồn tại của Subject và User
            var subjectExists = await _context.Subjects.AnyAsync(s => s.SubjectsId == dto.SubjectId);
            if (!subjectExists)
            {
                throw new KeyNotFoundException($"Không tìm thấy môn học với ID {dto.SubjectId}.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
            {
                throw new KeyNotFoundException($"Không tìm thấy người dùng (giáo viên) với ID {dto.UserId}.");
            }

            // Lỗi 2: Kiểm tra thời gian bắt đầu phải trong tương lai
            if (dto.StartTime <= DateTime.UtcNow)
            {
                throw new ArgumentException("Thời gian bắt đầu của lớp học phải ở trong tương lai.");
            }

            var upcomingClass = new UpcomingClass
            {
                StartTime = dto.StartTime,
                Status = dto.Status ?? "Scheduled", // Gán giá trị mặc định nếu null
                SubjectId = dto.SubjectId,
                UserId = dto.UserId
            };

            _context.UpcomingClasses.Add(upcomingClass);
            await _context.SaveChangesAsync();

            return new UpcomingClassDto
            {
                ClassId = upcomingClass.ClassId,
                StartTime = upcomingClass.StartTime,
                Status = upcomingClass.Status,
                SubjectId = upcomingClass.SubjectId,
                UserId = upcomingClass.UserId
            };
        }

        public async Task<IEnumerable<UpcomingClassDto>> GetAllAsync()
        {
            return await _context.UpcomingClasses.AsNoTracking().Select(uc => new UpcomingClassDto { ClassId = uc.ClassId, StartTime = uc.StartTime, Status = uc.Status, SubjectId = uc.SubjectId, UserId = uc.UserId }).ToListAsync();
        }

        public async Task<UpcomingClassDto?> GetByIdAsync(int id)
        {
            return await _context.UpcomingClasses.AsNoTracking().Where(uc => uc.ClassId == id).Select(uc => new UpcomingClassDto { ClassId = uc.ClassId, StartTime = uc.StartTime, Status = uc.Status, SubjectId = uc.SubjectId, UserId = uc.UserId }).FirstOrDefaultAsync();
        }
    }
}