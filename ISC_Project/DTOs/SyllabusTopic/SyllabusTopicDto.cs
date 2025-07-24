namespace ISC_Project.DTOs.SyllabusTopic
{
    public class SyllabusTopicDto
    {
        public int SyllabusId { get; set; }
        public string? TopicTitle { get; set; } = string.Empty!;
        public int? OrderIndex { get; set; }
        public string? Description { get; set; } = string.Empty!;
        public int? TeachingId { get; set; }
    }
}
