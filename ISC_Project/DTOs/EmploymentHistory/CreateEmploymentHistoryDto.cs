namespace ISC_Project.DTOs.EmploymentHistory
{
    public class CreateEmploymentHistoryDto
    {
        public string? Status { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? Note { get; set; }
        public string? Certificate { get; set; }
        public string? Form { get; set; }
        public string? DecidedRetireUrl { get; set; }
        public int? CreatedById { get; set; }
        public int? UserId { get; set; }
    }
} 