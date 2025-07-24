using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.AcceptingSchoolTransfer
{
    public class UpdateAcceptingSchoolTransferDto
    {
     

        public DateTime? MoveInDate { get; set; }
        [Range(1, 2, ErrorMessage = "Semester must be 1 or 2.")]
        public int? SemesterMoveIn { get; set; }
        [MaxLength(100)]
        public string? Province { get; set; } = string.Empty!;
        [MaxLength(100)]
        public string? District { get; set; } = string.Empty!;
        [MaxLength(255)]
        public string? ConvertFrom { get; set; } = string.Empty!;
        public string? Reason { get; set; } = string.Empty!;
        public string? FileUrl { get; set; } = string.Empty!;
        public int? UserId { get; set; }
        public int? SchoolYearId { get; set; }
    }
}
