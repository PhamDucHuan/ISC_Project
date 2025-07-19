using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.WorkHistory
{
    public class CreateWorkHistoryDto
    {
        [Required]
        [StringLength(255)]
        public string OrganizationName { get; set; } = null!;

        [StringLength(100)]
        public string? Department { get; set; }

        [StringLength(100)]
        public string? Position { get; set; }

        public DateTime? StarTime { get; set; }
        public DateTime? Endtime { get; set; }
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
