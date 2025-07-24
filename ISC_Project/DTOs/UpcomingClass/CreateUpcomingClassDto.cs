using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.UpcomingClass
{
    public class CreateUpcomingClassDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [StringLength(100)]
        public string? Status { get; set; } = string.Empty!;

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
