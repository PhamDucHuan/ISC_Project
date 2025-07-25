using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.FacultyStudyBlock
{
    public class CreateFacultyDto
    {
        [Required(ErrorMessage = "Tên khối học không được bỏ trống")]
        public string FacultyName { get; set; } = null!;

        [Required(ErrorMessage = "Mã khối học không được bỏ trống")]
        public string FacultyCode { get; set; } = null!;
    }
}
