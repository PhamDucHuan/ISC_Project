using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Student
{
    public class StudentProfileDto
    {
        public int StudentsProfileId { get; set; }
        public string StudentName { get; set; } = string.Empty!;
        public string StudentCode { get; set; } = string.Empty!;
        public string Status { get; set; } = string.Empty!;
    }

    public class CreateStudentProfileDto
    {
        [Required]
        [StringLength(255)]
        public string StudentName { get; set; } = string.Empty!;

        [Required]
        [StringLength(50)]
        public string StudentCode { get; set; } = string.Empty!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty!;

        [Required]
        public int ClassId { get; set; }
    }
}