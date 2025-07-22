using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IClassroomSettingService
    {
        Task<IEnumerable<ClassroomSetting>> GetAllAsync();
        Task<ClassroomSetting?> GetByIdAsync(int id);
        Task<ClassroomSetting> CreateAsync(ClassroomSetting setting);
    }
}
