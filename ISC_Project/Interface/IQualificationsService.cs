using ISC_Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Services
{
    public interface IQualificationsService
    {
        Task<IEnumerable<Qualification>> GetAllAsync();
        Task<Qualification?> GetByIdAsync(int id);
        Task<Qualification> CreateAsync(Qualification model);
        Task UpdateAsync(Qualification model);
    }
}