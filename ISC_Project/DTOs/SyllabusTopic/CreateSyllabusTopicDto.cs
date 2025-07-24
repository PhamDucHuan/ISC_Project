using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.SyllabusTopic
{
    public class CreateSyllabusTopicDto
    {
        [Required]
        [StringLength(255)]
        public string TopicTitle { get; set; } = string.Empty!;

        public int? OrderIndex { get; set; }
        public string? Description { get; set; } = string.Empty!;

        [Required]
        public int TeachingId { get; set; }

        public int? SchoolYearId { get; set; }
    }
}
