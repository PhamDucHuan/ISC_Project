namespace ISC_Project.DTOs.TrainingLevel
{
    public class TrainingLevelDto
    {
        public int TrainingId { get; set; }
        public string? TrainingName { get; set; } = string.Empty!;
        public string? TrainingForm { get; set; } = string.Empty!;
        public int? DurationYears { get; set; }
    }
}
