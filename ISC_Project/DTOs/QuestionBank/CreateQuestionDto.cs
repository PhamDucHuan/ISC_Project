using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.QuestionBank
{
    public class CreateQuestionDto
    {
        [Required]
        public string QuestionText { get; set; } = string.Empty!;

        [Required]
        [RegularExpression("^(SingleChoice|MultipleChoice|FillInTheBlank)$", ErrorMessage = "QuestionType không hợp lệ.")]
        public string QuestionType { get; set; } = string.Empty!;

        public int? SubjectId { get; set; }

        [Required]
        public List<CreateQuestionOptionDto> Options { get; set; } = new();
    }

    public class CreateQuestionOptionDto
    {
        [Required]
        public string OptionText { get; set; } = string.Empty!;
        public bool IsCorrect { get; set; }
    }
}
