using ISC_Project.DTOs.RelativesInformation;
using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IRelativesInformationService
    {
        Task<RelativesInformationDto?> GetByIdAsync(int relativesInformationId);
        Task<IEnumerable<RelativesInformationDto>> GetAllAsync();
        Task<RelativesInformation> CreateAsync(CreateRelativesInformationDto dto);
    }
}