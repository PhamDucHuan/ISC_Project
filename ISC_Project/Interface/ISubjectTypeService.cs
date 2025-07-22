using ISC_Project.DTOs.SubjectType;
using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface ISubjectTypeService
    {
        Task<SubjectTypeDto?> GetByIdAsync(int subjectTypeId);
        Task<IEnumerable<SubjectTypeDto>> GetAllAsync();
        Task<SubjectType> CreateAsync(CreateSubjectTypeDto dto);
    }
}