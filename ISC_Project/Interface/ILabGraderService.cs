using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface ILabGraderService
    {
        Task<IEnumerable<LabGrader>> GetAllAsync();
        Task<LabGrader?> GetByKeyAsync(int labId, int userId);
        Task<LabGrader> AddAsync(LabGrader entity);
    }
}
