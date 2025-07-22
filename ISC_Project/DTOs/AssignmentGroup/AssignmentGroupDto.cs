namespace ISC_Project.DTOs.AssignmentGroup
{
    public class AssignmentGroupDto
    {
        public int AssignmentsId { get; set; }
        public string? AssignmentTitle { get; set; } // Optional: To display associated assignment title
        public int ClassId { get; set; }
        public string? ClassName { get; set; }       // Optional: To display associated class name
    }
}
