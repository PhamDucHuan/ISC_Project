namespace ISC_Project.DTOs.SyllabusTopic
{
    public class SyllabusTopicDto
    {
        public int SyllabusId { get; set; }
        public string? TopicTitle { get; set; }
        public int? OrderIndex { get; set; }
        public string? Description { get; set; }
        public int? TeachingId { get; set; }
    }
}
