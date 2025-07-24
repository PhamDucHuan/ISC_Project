namespace ISC_Project.DTOs.SubjectType
{
    public class SubjectTypeDto
    {
        public int SubjectTypeId { get; set; }
        public string? SubjectTypeName { get; set; } = string.Empty!;
        public string? Description { get; set; } = string.Empty!;
        public bool? Status { get; set; }
        public string? Note { get; set; } = string.Empty!;
    }
}