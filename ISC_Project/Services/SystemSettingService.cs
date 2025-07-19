using ISC_Project.Data;
using ISC_Project.DTOs.SystemSetting;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly ISC_ProjectDbContext _context;

        public SystemSettingService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<SystemSettingDto> CreateAsync(CreateSystemSettingDto dto)
        {
            // Lỗi 1: Kiểm tra khóa chính đã tồn tại hay chưa
            var keyExists = await _context.SystemSettings.AnyAsync(s => s.SettingKey == dto.SettingKey);
            if (keyExists)
            {
                throw new ArgumentException($"Cài đặt với khóa '{dto.SettingKey}' đã tồn tại.");
            }

            var systemSetting = new SystemSetting
            {
                SettingKey = dto.SettingKey,
                SettingValue = dto.SettingValue,
                Description = dto.Description
            };

            _context.SystemSettings.Add(systemSetting);
            await _context.SaveChangesAsync();

            return new SystemSettingDto { SettingKey = systemSetting.SettingKey, SettingValue = systemSetting.SettingValue, Description = systemSetting.Description };
        }

        public async Task<IEnumerable<SystemSettingDto>> GetAllAsync()
        {
            return await _context.SystemSettings.AsNoTracking().Select(s => new SystemSettingDto { SettingKey = s.SettingKey, SettingValue = s.SettingValue, Description = s.Description }).ToListAsync();
        }

        public async Task<SystemSettingDto?> GetByIdAsync(string key)
        {
            return await _context.SystemSettings.AsNoTracking().Where(s => s.SettingKey == key).Select(s => new SystemSettingDto { SettingKey = s.SettingKey, SettingValue = s.SettingValue, Description = s.Description }).FirstOrDefaultAsync();
        }
    }
}