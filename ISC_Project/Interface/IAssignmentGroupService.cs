using ISC_Project.DTOs;
using ISC_Project.DTOs.AssignmentGroup;

namespace ISC_Project.Services
{
    public interface IAssignmentGroupService
    {
        Task<IEnumerable<AssignmentGroupDto>> GetAllAsync();
        // Get by composite key
        Task<AssignmentGroupDto?> GetByIdAsync(int assignmentId, int classId);
        Task<AssignmentGroupDto> CreateAsync(CreateAssignmentGroupDto createDto);
    }
}