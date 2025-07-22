namespace ISC_Project.DTOs.Assignment
{
    public class AssignmentDto
    {
        public int AssignmentId { get; set; }
        public string? Title { get; set; }
        public string? Format { get; set; }
        public string? AssignmentScope { get; set; }
        public string? Category { get; set; }
        public DateTime? StarTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? AssignmentUrl { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? PartitionType { get; set; }
        public DateTime? CraeteAt { get; set; }
        public int? TeachingId { get; set; }
        public string? TeachingAssignmentType { get; set; } // Changed from TeachingAssessmentTitle to reflect AssignmentType
        public int? FacultyId { get; set; }

        // public ICollection<AssessmentPartDto>? AssessmentParts { get; set; } // No change here
    }
}
