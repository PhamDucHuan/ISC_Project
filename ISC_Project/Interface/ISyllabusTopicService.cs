using ISC_Project.DTOs.SyllabusTopic;

namespace ISC_Project.Interface
{
    public interface ISyllabusTopicService
    {
        Task<SyllabusTopicDto?> GetByIdAsync(int id);
        Task<IEnumerable<SyllabusTopicDto>> GetAllAsync();
        Task<SyllabusTopicDto> CreateAsync(CreateSyllabusTopicDto dto);
    }
}
