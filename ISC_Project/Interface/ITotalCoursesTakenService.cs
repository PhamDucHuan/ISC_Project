using ISC_Project.DTOs.TotalCoursesTaken;

namespace ISC_Project.Interface
{
    public interface ITotalCoursesTakenService
    {
        Task<TotalCoursesTakenDto?> GetByIdAsync(int id);
        Task<IEnumerable<TotalCoursesTakenDto>> GetAllAsync();
        Task<TotalCoursesTakenDto> CreateAsync(CreateTotalCoursesTakenDto dto);
    }
}
