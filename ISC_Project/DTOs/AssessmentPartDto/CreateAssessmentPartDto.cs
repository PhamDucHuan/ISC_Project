using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.AssessmentPartDto
{
    public class CreateAssessmentPartDto
    {
        [Required(ErrorMessage = "Part order is required.")]
        public int? PartOrder { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; } = string.Empty!;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; } = string.Empty!;

        [StringLength(500, ErrorMessage = "Assignment URL cannot exceed 500 characters.")]
        [Url(ErrorMessage = "Assignment URL must be a valid URL.")] // Optional: Basic URL validation
        public string? AssignmentUrl { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        public DateTime? StarTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        public DateTime? EndTime { get; set; }

        // [Required(ErrorMessage = "Assignment ID is required.")] // Make required if an AssessmentPart must always belong to an Assignment
        public int? AssignmentId { get; set; }
    }
}
