using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface ICoursesLearnedService
    {
        Task<IEnumerable<CoursesLearned>> GetAllAsync();
        Task<CoursesLearned?> GetByIdAsync(int id);
        Task<CoursesLearned> CreateAsync(CoursesLearned course);
    }
}
