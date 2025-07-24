using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.SchoolYear
{
    public class SchoolYearDto
    {
        public int SchoolYearId { get; set; }
        public string SchoolYearName { get; set; } = string.Empty!;
        public DateTime? StarTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    public class CreateSchoolYearDto
    {
        [Required]
        [StringLength(100)]
        public string SchoolYearName { get; set; } = string.Empty!;
        [Required]
        public DateTime StarTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public int SchoolId { get; set; }
    }
}