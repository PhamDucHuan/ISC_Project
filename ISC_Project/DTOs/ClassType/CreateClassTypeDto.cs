using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.ClassType
{
    public class ClassTypeDto
    {
        public int ClassTypeId { get; set; }
        public string ClassTypeName { get; set; } = string.Empty;
    }

    public class CreateClassTypeDto
    {
        [Required]
        [StringLength(255)]
        public string ClassTypeName { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}