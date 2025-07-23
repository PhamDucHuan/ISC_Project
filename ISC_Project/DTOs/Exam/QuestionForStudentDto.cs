namespace ISC_Project.DTOs.Exam
{
    public class QuestionForStudentDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = null!;
        public List<OptionDto> Options { get; set; } = new List<OptionDto>();
    }
    public class OptionDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; } = null!;
    }
}
