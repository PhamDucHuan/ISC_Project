using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.AssessmentPartDto
{
    public class UpdateAssessmentPartDto
    {
        // ID is typically passed in the URL for PUT, not in the body.

        [Range(1, int.MaxValue, ErrorMessage = "Part order must be a positive integer.")]
        public int? PartOrder { get; set; }

        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Assignment URL cannot exceed 500 characters.")]
        [Url(ErrorMessage = "Assignment URL must be a valid URL.")]
        public string? AssignmentUrl { get; set; }

        public DateTime? StarTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? AssignmentId { get; set; }
    }
}
