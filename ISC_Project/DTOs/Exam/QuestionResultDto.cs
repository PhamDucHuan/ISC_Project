namespace ISC_Project.DTOs.Exam
{
    public class QuestionResultDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;

        public int YourAnswerOptionId { get; set; }
        public string YourAnswerText { get; set; } = string.Empty;

        public int CorrectAnswerOptionId { get; set; }
        public string CorrectAnswerText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
