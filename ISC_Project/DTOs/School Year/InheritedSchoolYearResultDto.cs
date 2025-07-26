using ISC_Project.DTOs.Class;
using ISC_Project.DTOs.SchoolYear;
using ISC_Project.DTOs.Student;
using ISC_Project.DTOs.Subject;

namespace ISC_Project.DTOs.School_Year
{
    public class InheritedSchoolYearResultDto
    {
        // Information of the newly created school year
        public SchoolYearDto NewSchoolYear { get; set; } = null!;

        // List of classes copied
        public List<ClassDto> InheritedClasses { get; set; } = new List<ClassDto>();

        // Student list copied
        public List<StudentProfileDto> InheritedStudents { get; set; } = new List<StudentProfileDto>();

        public List<SubjectDto> InheritedSubjects { get; set; } = new List<SubjectDto>();
    }
}
