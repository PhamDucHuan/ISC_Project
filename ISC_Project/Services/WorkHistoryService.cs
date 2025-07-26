using ISC_Project.Data;
using ISC_Project.DTOs.WorkHistory;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class WorkHistoryService : IWorkHistoryService
    {
        private readonly ISC_ProjectDbContext _context;

        public WorkHistoryService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<WorkHistoryDto> CreateAsync(CreateWorkHistoryDto dto)
        {
            // Lỗi 1: Kiểm tra sự tồn tại của User
            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
            {
                throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {dto.UserId}.");
            }

            // Lỗi 2: Kiểm tra logic thời gian
            if (dto.StarTime.HasValue && dto.Endtime.HasValue && dto.StarTime.Value > dto.Endtime.Value)
            {
                throw new ArgumentException("Ngày bắt đầu không thể sau ngày kết thúc.");
            }

            var workHistory = new WorkHistory
            {
                OrganizationName = dto.OrganizationName,
                Department = dto.Department,
                Position = dto.Position,
                StarTime = dto.StarTime,
                Endtime = dto.Endtime,
                Description = dto.Description,
                UserId = dto.UserId
            };

            _context.WorkHistories.Add(workHistory);
            await _context.SaveChangesAsync();

            return new WorkHistoryDto { WordId = workHistory.WordId, OrganizationName = workHistory.OrganizationName, Position = workHistory.Position, StarTime = workHistory.StarTime, Endtime = workHistory.Endtime, UserId = workHistory.UserId };
        }

        public async Task<IEnumerable<WorkHistoryDto>> GetAllAsync()
        {
            return await _context.WorkHistories.AsNoTracking().Select(wh => new WorkHistoryDto { WordId = wh.WordId, OrganizationName = wh.OrganizationName, Position = wh.Position, StarTime = wh.StarTime, Endtime = wh.Endtime, UserId = wh.UserId }).ToListAsync();
        }

        public async Task<WorkHistoryDto?> GetByIdAsync(int id)
        {
            return await _context.WorkHistories.AsNoTracking().Where(wh => wh.WordId == id).Select(wh => new WorkHistoryDto { WordId = wh.WordId, OrganizationName = wh.OrganizationName, Position = wh.Position, StarTime = wh.StarTime, Endtime = wh.Endtime, UserId = wh.UserId }).FirstOrDefaultAsync();
        }
    }
}