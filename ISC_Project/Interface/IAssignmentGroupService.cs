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
        // Update method for composite key needs both old and new IDs
        Task<bool> UpdateAsync(int oldAssignmentId, int oldClassId, UpdateAssignmentGroupDto updateDto);
        Task<bool> DeleteAsync(int assignmentId, int classId);
    }
}