using ISC_Project.Models;

namespace ISC_Project.Repositories
{
    public interface IReservedRepository
    {
        Task<Reserved> GetByIdAsync(int reasonId);
        Task<IEnumerable<Reserved>> GetAllAsync();
    }
}