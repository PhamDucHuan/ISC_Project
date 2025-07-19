using ISC_Project.DTOs.UpcomingClass;

namespace ISC_Project.Interface
{
    public interface IUpcomingClassService
    {
        Task<UpcomingClassDto?> GetByIdAsync(int id);
        Task<IEnumerable<UpcomingClassDto>> GetAllAsync();
        Task<UpcomingClassDto> CreateAsync(CreateUpcomingClassDto dto);
    }
}
