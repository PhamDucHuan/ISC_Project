using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class LabGraderService : ILabGraderService
    {
        private readonly ISC_ProjectDbContext _context;

        public LabGraderService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task AssignGraderAsync(int? labScheduleId, int? userId)
        {
            // Kiểm tra xem đã tồn tại chưa để tránh lỗi khóa chính
            var existing = await _context.LabGraders
                .AnyAsync(lg => lg.LabSchedulesId == labScheduleId && lg.UserId == userId);

            if (!existing)
            {
                var labGrader = new LabGrader
                {
                    LabSchedulesId = labScheduleId,
                    UserId = userId
                };
                _context.LabGraders.Add(labGrader);
                await _context.SaveChangesAsync();
            }
        }

        // READ
        public async Task<IEnumerable<User>> GetGradersForLabAsync(int labScheduleId)
        {
            // Dùng LINQ để join và lấy thông tin User
            var query = from lg in _context.LabGraders
                        join u in _context.Users on lg.UserId equals u.UserId
                        where lg.LabSchedulesId == labScheduleId
                        select u;

            return await query.ToListAsync();
        }
    }
}
