using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Subject
{
    public class SubjectDto
    {
        public int SubjectsId { get; set; }
        public string SubjectsName { get; set; } = string.Empty!;
        public string SubjectCode { get; set; } = string.Empty!;
    }

    public class CreateSubjectDto
    {
        [Required]
        [StringLength(255)]
        public string SubjectsName { get; set; } = string.Empty!;

        [Required]
        [StringLength(50)]
        public string SubjectCode { get; set; } = string.Empty!;

        [Required]
        public int DepartmentId { get; set; }
    }
}