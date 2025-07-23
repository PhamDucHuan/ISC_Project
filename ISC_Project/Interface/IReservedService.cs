using ISC_Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Services
{
    public interface IReservedService
    {
        Task<IEnumerable<Reserved>> GetAllAsync();
        Task<Reserved?> GetByIdAsync(int id);
        Task<Reserved> CreateAsync(Reserved model);
        Task UpdateAsync(Reserved model);
    }
}