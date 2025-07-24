namespace ISC_Project.DTOs.AcceptingSchoolTransfer
{
    public class AcceptingSchoolTransferDto
    {
        public int AcceptingSchoolTransfersId { get; set; }
        public DateTime? MoveInDate { get; set; }
        public int? SemesterMoveIn { get; set; }
        public string? Province { get; set; } = string.Empty!;
        public string? District { get; set; } = string.Empty!;
        public string? ConvertFrom { get; set; } = string.Empty!;
        public string? Reason { get; set; } = string.Empty!;
        public string? FileUrl { get; set; } = string.Empty!;
        public int? UserId { get; set; }
        public int? SchoolYearId { get; set; }
     
    }
}
