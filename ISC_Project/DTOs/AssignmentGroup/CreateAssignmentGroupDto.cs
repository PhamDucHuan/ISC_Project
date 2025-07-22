using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.AssignmentGroup
{
    public class CreateAssignmentGroupDto
    {
        [Required(ErrorMessage = "Assignment ID is required.")]
        public int AssignmentsId { get; set; }

        [Required(ErrorMessage = "Class ID is required.")]
        public int ClassId { get; set; }
    }
}
