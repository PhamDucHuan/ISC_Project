using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IStudentsChangeClassService
    {
        Task<IEnumerable<StudentsChangeClass>> GetAllAsync();
        Task<StudentsChangeClass> AddAsync(StudentsChangeClass changeRequest);
        Task<StudentsChangeClass?> GetByIdAsync(int id);

    }
}
