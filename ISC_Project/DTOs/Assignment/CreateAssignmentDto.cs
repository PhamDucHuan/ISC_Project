using System;
using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs
{
    public class CreateAssignmentDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Format cannot exceed 50 characters.")]
        public string? Format { get; set; }

        [StringLength(255, ErrorMessage = "Assignment Scope cannot exceed 255 characters.")]
        public string? AssignmentScope { get; set; }

        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters.")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        public DateTime? StarTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        public DateTime? EndTime { get; set; }

        [StringLength(500, ErrorMessage = "Assignment URL cannot exceed 500 characters.")]
        [Url(ErrorMessage = "Assignment URL must be a valid URL.")] // Optional: Basic URL validation
        public string? AssignmentUrl { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string? Status { get; set; }

        [StringLength(50, ErrorMessage = "Partition Type cannot exceed 50 characters.")]
        public string? PartitionType { get; set; }

        public int? TeachingId { get; set; }
        public int? FacultyId { get; set; }
    }
}