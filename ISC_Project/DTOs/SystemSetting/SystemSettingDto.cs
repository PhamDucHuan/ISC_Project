namespace ISC_Project.DTOs.SystemSetting
{
    public class SystemSettingDto
    {
        public string SettingKey { get; set; } = string.Empty!;
        public string? SettingValue { get; set; } = string.Empty!;
        public string? Description { get; set; } = string.Empty!;
    }
}
