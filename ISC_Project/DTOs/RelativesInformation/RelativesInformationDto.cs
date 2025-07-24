namespace ISC_Project.DTOs.RelativesInformation
{
    public class RelativesInformationDto
    {
        public int RelativesInformationId { get; set; }
        public string? RelativesName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public int? RegistrationsId { get; set; }
    }
}