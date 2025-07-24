using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.UserManagement
{
    public class UpdateUserDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty!;
        [Required]
        public int RoleId { get; set; }
    }
}
