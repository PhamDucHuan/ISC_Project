using ISC_Project.Models;

namespace ISC_Project.Repositories
{
    public interface IQualificationsRepository
    {
        Task<Qualification> CreateAsync(Qualification qualifications);
        Task<Qualification> GetByIdAsync(int qualificationsId);
        Task<IEnumerable<Qualification>> GetAllAsync();
    }
}