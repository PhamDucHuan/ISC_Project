using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.SystemSetting
{
    public class CreateSystemSettingDto
    {
        [Required]
        [StringLength(100)]
        public string SettingKey { get; set; } = string.Empty!;
        public string? SettingValue { get; set; } = string.Empty!;

        [StringLength(255)]
        public string? Description { get; set; } = string.Empty!;
    }
}
