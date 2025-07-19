using ISC_Project.DTOs.SystemSetting;

namespace ISC_Project.Interface
{
    public interface ISystemSettingService
    {
        Task<SystemSettingDto?> GetByIdAsync(string key);
        Task<IEnumerable<SystemSettingDto>> GetAllAsync();
        Task<SystemSettingDto> CreateAsync(CreateSystemSettingDto dto);
    }
}
