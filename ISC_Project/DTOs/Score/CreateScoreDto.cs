namespace ISC_Project.DTOs.Score
{
    public class CreateScoreDto
    {
        public string? ScoreType { get; set; }
        public string? Coefficient { get; set; }
        public float? ScoreNumber { get; set; }
        public float? AverageScore { get; set; }
        public string? Semester { get; set; }
        public int? SubjectsId { get; set; }
        public int? ClassId { get; set; }
        public int? SchoolYearId { get; set; }
    }
}
