using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.TrainingLevel
{
    public class CreateTrainingLevelDto
    {
        [Required]
        [StringLength(255)]
        public string TrainingName { get; set; } = null!;
        public string? TrainingForm { get; set; }
        public bool? IsCreditBased { get; set; }
        public int? DurationYears { get; set; }
        public int? RequiredCredits { get; set; }
        public int? SchoolYearId { get; set; }
    }
}
