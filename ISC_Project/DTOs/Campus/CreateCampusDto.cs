using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Campus
{
    public class CreateCampusDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = null!;

        [StringLength(512)]
        public string? Address { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public int SchoolId { get; set; }
    }
}
