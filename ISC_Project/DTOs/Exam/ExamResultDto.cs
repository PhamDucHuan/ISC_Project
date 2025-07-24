namespace ISC_Project.DTOs.Exam
{
    public class ExamResultDto
    {
        public int SubmissionId { get; set; }
        public double FinalScore { get; set; }
        public string Message { get; set; } = "Đã nộp bài thành công!";
        public List<QuestionResultDto> Results { get; set; } = new List<QuestionResultDto>();
    }
}
