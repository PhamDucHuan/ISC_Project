using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.AssessmentQuestion
{
    public class UpdateAssessmentQuestionDto
    {

        [Range(1, int.MaxValue, ErrorMessage = "Question Order must be a positive integer.")]
        public int? QuestionOrder { get; set; }

        public int? QuestionsId { get; set; }
    }
}
