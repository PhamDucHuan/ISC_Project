using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Teacher
{
    // DTO to return
    public class TeacherProfileDto
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string TeacherCode { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    // DTO to create new
    public class CreateTeacherProfileDto
    {
        [Required]
        [StringLength(255)]
        public string TeacherName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TeacherCode { get; set; } = string.Empty;

        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [EmailAddress]
        public string Email { get; set; } = string.Empty    ;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public int Department_ID { get; set; }
    }
}