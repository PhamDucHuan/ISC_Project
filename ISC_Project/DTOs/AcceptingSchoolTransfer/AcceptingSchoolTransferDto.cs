namespace ISC_Project.DTOs.AcceptingSchoolTransfer
{
    public class AcceptingSchoolTransferDto
    {
        public int AcceptingSchoolTransfersId { get; set; }
        public DateTime? MoveInDate { get; set; }
        public int? SemesterMoveIn { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? ConvertFrom { get; set; }
        public string? Reason { get; set; }
        public string? FileUrl { get; set; }
        public int? UserId { get; set; }
        public int? SchoolYearId { get; set; }
     
    }
}
