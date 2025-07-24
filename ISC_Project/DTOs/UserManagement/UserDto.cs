namespace ISC_Project.DTOs.UserManagement
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; } = string.Empty!;
        public string? FullName { get; set; } = string.Empty!;
        public string? Email { get; set; } = string.Empty!;
        public string Status { get; set; } = string.Empty!;
        public string RoleName { get; set; } = string.Empty!;
    }
}
