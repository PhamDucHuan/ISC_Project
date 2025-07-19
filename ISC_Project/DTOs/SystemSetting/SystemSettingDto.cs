namespace ISC_Project.DTOs.SystemSetting
{
    public class SystemSettingDto
    {
        public string SettingKey { get; set; } = null!;
        public string? SettingValue { get; set; }
        public string? Description { get; set; }
    }
}
