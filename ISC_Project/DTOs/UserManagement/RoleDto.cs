namespace ISC_Project.DTOs.UserManagement
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty!;
        public string? Description { get; set; } = string.Empty!;
        public bool IsAdmin { get; set; }
    }
}
