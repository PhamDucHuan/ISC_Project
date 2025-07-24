using ISC_Project.DTOs.Class;
using ISC_Project.DTOs.SchoolYear;
using ISC_Project.DTOs.Student;

namespace ISC_Project.DTOs.School_Year
{
    public class InheritedSchoolYearResultDto
    {
        // Information of the newly created school year
        public SchoolYearDto NewSchoolYear { get; set; } = string.Empty!;

        // List of classes copied
        public List<ClassDto> InheritedClasses { get; set; } = new List<ClassDto>();

        // Student list copied
        public List<StudentProfileDto> InheritedStudents { get; set; } = new List<StudentProfileDto>();
    }
}
