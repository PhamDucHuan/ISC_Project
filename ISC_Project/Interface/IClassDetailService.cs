using ISC_Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Interface
{
    public interface IClassDetailService
    {
        Task<IEnumerable<ClassDetail>> GetAllAsync();
        Task<ClassDetail?> GetByIdAsync(int id);
        Task<ClassDetail> CreateAsync(ClassDetail detail);
        Task<bool> UpdateAsync(int id, ClassDetail detail);
        Task<bool> DeleteAsync(int id);
    }
}
