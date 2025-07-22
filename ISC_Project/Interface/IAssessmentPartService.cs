using ISC_Project.DTOs;
using ISC_Project.DTOs.AssessmentPartDto;

using ISC_Project.Models; 
namespace ISC_Project.Services
{
    public interface IAssessmentPartService
    {
        Task<IEnumerable<AssessmentPartDto>> GetAllAsync();
        Task<AssessmentPartDto?> GetByIdAsync(int id);
        Task<AssessmentPartDto> CreateAsync(CreateAssessmentPartDto createDto); // KEEP this one
    }
}