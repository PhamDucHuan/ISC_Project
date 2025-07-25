using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IPastClassesService
    {
        Task<IEnumerable<PastClass>> GetAllAsync();
        Task<PastClass?> GetByIdAsync(int classId);
        Task<PastClass> CreateAsync(PastClass newClass);
    }
}
