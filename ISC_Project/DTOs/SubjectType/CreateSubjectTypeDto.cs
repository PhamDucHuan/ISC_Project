using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.SubjectType
{
    public class CreateSubjectTypeDto
    {
        [Required(ErrorMessage = "Tên loại môn học không được để trống")]
        [MaxLength(255)]
        public string? SubjectTypeName { get; set; } = string.Empty!;

        public string? Description { get; set; } = string.Empty!;

        public bool? Status { get; set; }

        public string? Note { get; set; }
    }
}