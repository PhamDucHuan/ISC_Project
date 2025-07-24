using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.RelativesInformation
{
    public class CreateRelativesInformationDto
    {
        [Required(ErrorMessage = "Tên người thân không được để trống")]
        [MaxLength(255)]
        public string? RelativesName { get; set; } = string.Empty!;

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string? Address { get; set; } = string.Empty!;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; } = string.Empty!;

        [Required(ErrorMessage = "ID đăng ký không được để trống")]
        public int? RegistrationsId { get; set; }
    }
}