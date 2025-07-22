using ISC_Project.DTOs.AcceptingSchoolTransfer;

namespace ISC_Project.Interface
{
    public interface IAcceptingSchoolTransferService
    {
        Task<IEnumerable<AcceptingSchoolTransferDto>> GetAllAsync();
        Task<AcceptingSchoolTransferDto?> GetByIdAsync(int id);
        Task<AcceptingSchoolTransferDto> CreateAsync(CreateAcceptingSchoolTransferDto createDto);
    }
}
