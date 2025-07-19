using ISC_Project.DTOs.WorkHistory;

namespace ISC_Project.Interface
{
    public interface IWorkHistoryService
    {
        Task<WorkHistoryDto?> GetByIdAsync(int id);
        Task<IEnumerable<WorkHistoryDto>> GetAllAsync();
        Task<WorkHistoryDto> CreateAsync(CreateWorkHistoryDto dto);
    }
}
