namespace ISC_Project.DTOs.Enrollment
{
    public class EnrollmentDto
    {
        public int EnrollmentsId { get; set; }
        public string? Status { get; set; }
        public int? UserId { get; set; }
        public int? ClassId { get; set; }
    }
}
