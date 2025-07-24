using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.WorkHistory
{
    public class CreateWorkHistoryDto
    {
        [Required]
        [StringLength(255)]
        public string OrganizationName { get; set; } = string.Empty!;

        [StringLength(100)]
        public string? Department { get; set; } = string.Empty!;

        [StringLength(100)]
        public string? Position { get; set; } = string.Empty!;

        public DateTime? StarTime { get; set; }
        public DateTime? Endtime { get; set; }
        public string? Description { get; set; } = string.Empty!;

        [Required]
        public int UserId { get; set; }
    }
}
