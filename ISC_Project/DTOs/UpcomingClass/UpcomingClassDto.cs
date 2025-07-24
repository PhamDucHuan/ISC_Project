namespace ISC_Project.DTOs.UpcomingClass
{
    public class UpcomingClassDto
    {
        public int ClassId { get; set; }
        public DateTime? StartTime { get; set; }
        public string? Status { get; set; } = string.Empty!;
        public int? SubjectId { get; set; }
        public int? UserId { get; set; }
    }
}
