using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.UserManagement
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty!;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty!;
        [Required]
        public string FullName { get; set; } = string.Empty!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty!;
        [Required]
        public int RoleId { get; set; }
    }
}
