namespace ISC_Project.DTOs.AssessmentPartDto
{
    public class AssessmentPartDto
    {
        public int AssessmentPartsId { get; set; }
        public int? PartOrder { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AssignmentUrl { get; set; }
        public DateTime? StarTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? AssignmentId { get; set; }
        public string? AssignmentTitle { get; set; } 
    }
}
