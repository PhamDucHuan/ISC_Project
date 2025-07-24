using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.FamilyInformation
{
    public class CreateFamilyInformationDto
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? FamilyName { get; set; }
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateTime? BirthOfFamily { get; set; }
        [Required(ErrorMessage = "Nghề nghiệp không được để trống")]
        public string? JobFamily { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string? PhoneFamily { get; set; }
        [Required(ErrorMessage = "Loại quan hệ không được để trống")]
        public string? FamilyType { get; set; }
    }
}
