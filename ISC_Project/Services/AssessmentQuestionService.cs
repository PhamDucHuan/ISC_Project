using Microsoft.EntityFrameworkCore;
using ISC_Project.Data;
using ISC_Project.Models;
using ISC_Project.DTOs;
using ISC_Project.DTOs.AssessmentQuestion.ISC_Project.DTOs;
using ISC_Project.DTOs.AssessmentQuestion;

namespace ISC_Project.Services
{
    public class AssessmentQuestionService : IAssessmentQuestionService
    {
        private readonly ISC_ProjectDbContext _context;

        public AssessmentQuestionService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssessmentQuestionDto>> GetAllAsync()
        {
            return await _context.AssessmentQuestions
                                 .Include(aq => aq.Questions)
                                 .Select(aq => new AssessmentQuestionDto
                                 {
                                     AssessmentQuestionsId = aq.AssessmentQuestionsId,
                                     QuestionOrder = aq.QuestionOrder,
                                     QuestionsId = aq.QuestionsId,
                                     QuestionsText = aq.Questions != null ? aq.Questions.QuestionsText : null
                                 })
                                 .ToListAsync();
        }

        public async Task<AssessmentQuestionDto?> GetByIdAsync(int id)
        {
            var aq = await _context.AssessmentQuestions
                                   .Include(aq => aq.Questions) // Cần include để truy cập QuestionsText
                                   .FirstOrDefaultAsync(aq => aq.AssessmentQuestionsId == id);
            if (aq == null)
            {
                return null;
            }

            return new AssessmentQuestionDto
            {
                AssessmentQuestionsId = aq.AssessmentQuestionsId,
                QuestionOrder = aq.QuestionOrder,
                QuestionsId = aq.QuestionsId,
                QuestionsText = aq.Questions != null ? aq.Questions.QuestionsText : null // Đã sửa
            };
        }

        public async Task<AssessmentQuestionDto> CreateAsync(CreateAssessmentQuestionDto createDto)
        {
            var aq = new AssessmentQuestion
            {
                QuestionOrder = createDto.QuestionOrder,
                QuestionsId = createDto.QuestionsId
            };

            _context.AssessmentQuestions.Add(aq);
            await _context.SaveChangesAsync();

            // Fetch the related Question to include its text in the returned DTO
            var createdAq = await _context.AssessmentQuestions
                                          .Include(x => x.Questions) // Cần include để truy cập QuestionsText
                                          .FirstOrDefaultAsync(x => x.AssessmentQuestionsId == aq.AssessmentQuestionsId);

            return new AssessmentQuestionDto
            {
                AssessmentQuestionsId = createdAq.AssessmentQuestionsId,
                QuestionOrder = createdAq.QuestionOrder,
                QuestionsId = createdAq.QuestionsId,
                QuestionsText = createdAq.Questions != null ? createdAq.Questions.QuestionsText : null // Đã sửa
            };
        }
    }
}