using ISC_Project.DTOs.Campus;

namespace ISC_Project.Interface
{
    public interface ICampusService
    {
        Task<CampusDto?> GetByIdAsync(int id);
        Task<IEnumerable<CampusDto>> GetAllAsync();
        Task<CampusDto> CreateAsync(CreateCampusDto dto);
    }
}
