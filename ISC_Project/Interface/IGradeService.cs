using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> GetAllGradesAsync();
        Task<Grade> GetGradeByIdAsync(int id);
        Task<Grade> CreateGradeAsync(Grade grade);

    }
}
