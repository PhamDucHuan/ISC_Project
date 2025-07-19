using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.TotalCoursesTaken
{
    public class CreateTotalCoursesTakenDto
    {
        [Required]
        public int UserId { get; set; }

        public int? TotalNumberCourses { get; set; }

        public decimal? TotalPayment { get; set; }

        public int? CoursesLearnedId { get; set; }
    }
}
