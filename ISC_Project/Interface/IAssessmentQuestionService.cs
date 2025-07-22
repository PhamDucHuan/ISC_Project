using ISC_Project.DTOs;
using ISC_Project.DTOs.AssessmentQuestion.ISC_Project.DTOs;
using ISC_Project.DTOs.AssessmentQuestion;

namespace ISC_Project.Services
{
    public interface IAssessmentQuestionService
    {
        Task<IEnumerable<AssessmentQuestionDto>> GetAllAsync();
        Task<AssessmentQuestionDto?> GetByIdAsync(int id);
        Task<AssessmentQuestionDto> CreateAsync(CreateAssessmentQuestionDto createDto);
    }
}