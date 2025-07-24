using ISC_Project.DTOs.FamilyInformation;
using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IFamilyInformationService
    {
        Task<IEnumerable<IFamilyInformationService>> GetAllFamilyInformationAsync();
        Task<FamilyInformation?> GetFamilyInformationByIdAsync(int id);
        Task<FamilyInformation?> CreateFamilyInformationAsync(CreateFamilyInformationDto dto);
    }
}
