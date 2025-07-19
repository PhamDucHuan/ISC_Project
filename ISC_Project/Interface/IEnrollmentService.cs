using ISC_Project.DTOs.Enrollment;

namespace ISC_Project.Interface
{
    public interface IEnrollmentService
    {
        Task<EnrollmentDto?> GetByIdAsync(int id);
        Task<IEnumerable<EnrollmentDto>> GetAllAsync();
        Task<EnrollmentDto> CreateAsync(CreateEnrollmentDto dto);
    }
}
