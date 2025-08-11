using ISC_Project.DTOs.FacultyStudyBlock;
using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IFacultyStudyBlockService
    {
        Task<IEnumerable<FacultyStudyBlock>> GetAllFacultyStudyBlocksAsync();

        Task<FacultyStudyBlock?> GetFacultyStudyBlockByIdAsync(int id);
        Task<FacultyStudyBlock> CreateFacultyStudyBlockAsync(CreateFacultyDto dto);
    }
}
