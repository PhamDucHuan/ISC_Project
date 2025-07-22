namespace ISC_Project.DTOs.AssessmentQuestion
{
    using System.ComponentModel.DataAnnotations;

    namespace ISC_Project.DTOs
    {
        public class CreateAssessmentQuestionDto
        {
            [Required(ErrorMessage = "Question Order is required.")]
            [Range(1, int.MaxValue, ErrorMessage = "Question Order must be a positive integer.")]
            public int QuestionOrder { get; set; } 

            [Required(ErrorMessage = "Question ID is required.")]
            public int QuestionsId { get; set; } 
        }
    }
}
