namespace ISC_Project.DTOs.WorkHistory
{
    public class WorkHistoryDto
    {
        public int WordId { get; set; }
        public string? OrganizationName { get; set; }
        public string? Position { get; set; }
        public DateTime? StarTime { get; set; }
        public DateTime? Endtime { get; set; }
        public int? UserId { get; set; }
    }
}
