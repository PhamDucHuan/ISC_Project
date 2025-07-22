using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.AssignmentGroup
{
    public class UpdateAssignmentGroupDto
    {
      

        [Required(ErrorMessage = "New Assignment ID is required.")]
        public int NewAssignmentsId { get; set; }

        [Required(ErrorMessage = "New Class ID is required.")]
        public int NewClassId { get; set; }
    }
}
