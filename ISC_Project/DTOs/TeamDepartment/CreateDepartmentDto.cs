using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Department
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }

    public class CreateDepartmentDto
    {
        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;

        [Required]
        public int SchoolId { get; set; }
    }
}