using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.AcceptingSchoolTransfer
{
    public class CreateAcceptingSchoolTransferDto
    {
        [Required(ErrorMessage = "Move In Date is required.")]
        public DateTime? MoveInDate { get; set; }
        [Range(1, 2, ErrorMessage = "Semester must be 1 or 2.")] 
        public int? SemesterMoveIn { get; set; }
        [MaxLength(100)]
        public string? Province { get; set; }
        [MaxLength(100)]
        public string? District { get; set; }
        [MaxLength(255)]
        public string? ConvertFrom { get; set; }
        public string? Reason { get; set; }
        public string? FileUrl { get; set; }
        public int? UserId { get; set; } 
        public int? SchoolYearId { get; set; } 
    }
}
