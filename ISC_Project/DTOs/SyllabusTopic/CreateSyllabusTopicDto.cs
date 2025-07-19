using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.SyllabusTopic
{
    public class CreateSyllabusTopicDto
    {
        [Required]
        [StringLength(255)]
        public string TopicTitle { get; set; } = null!;

        public int? OrderIndex { get; set; }
        public string? Description { get; set; }

        [Required]
        public int TeachingId { get; set; }

        public int? SchoolYearId { get; set; }
    }
}
