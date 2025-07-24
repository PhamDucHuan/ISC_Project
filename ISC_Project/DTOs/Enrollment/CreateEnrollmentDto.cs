using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Enrollment
{
    public class CreateEnrollmentDto
    {
        [Required]
        [StringLength(100)]
        public string Status { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ClassId { get; set; }

        public int? SchoolYearId { get; set; }
    }
}
