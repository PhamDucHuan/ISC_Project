using ISC_Project.DTOs.TrainingLevel;

namespace ISC_Project.Interface
{
    public interface ITrainingLevelService
    {
        Task<TrainingLevelDto?> GetByIdAsync(int id);
        Task<IEnumerable<TrainingLevelDto>> GetAllAsync();
        Task<TrainingLevelDto> CreateAsync(CreateTrainingLevelDto dto);
    }
}
