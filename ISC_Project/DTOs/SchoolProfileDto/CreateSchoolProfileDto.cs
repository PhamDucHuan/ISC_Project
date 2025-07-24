using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.School
{
    public class SchoolProfileDto
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; } = string.Empty!;
        public string SchoolCode { get; set; } = string.Empty!;
        public string Email { get; set; } = string.Empty!;
    }

    public class CreateSchoolProfileDto
    {
        [Required]
        [StringLength(255)]
        public string SchoolName { get; set; } = string.Empty!;

        [Required]
        [StringLength(50)]
        public string SchoolCode { get; set; } = string.Empty!;

        [EmailAddress]
        public string Email { get; set; } = string.Empty!;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty!;
    }
}