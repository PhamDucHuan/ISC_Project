using ISC_Project.DTOs.EmploymentHistory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Interface
{
    public interface IEmploymentHistoryService
    {
        Task<IEnumerable<EmploymentHistoryDto>> GetAllAsync();
        Task<EmploymentHistoryDto?> GetByIdAsync(int id);
        Task<EmploymentHistoryDto> CreateAsync(CreateEmploymentHistoryDto dto);
    }
} 