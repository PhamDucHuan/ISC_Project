using ISC_Project.DTOs.Class;
using ISC_Project.DTOs.SchoolYear;
using ISC_Project.DTOs.Student;

namespace ISC_Project.DTOs.School_Year
{
    public class InheritedSchoolYearResultDto
    {
        // Thông tin của niên khóa mới được tạo
        public SchoolYearDto NewSchoolYear { get; set; } = null!;

        // Danh sách các lớp học đã được sao chép
        public List<ClassDto> InheritedClasses { get; set; } = new List<ClassDto>();

        // Danh sách học viên đã được sao chép
        public List<StudentProfileDto> InheritedStudents { get; set; } = new List<StudentProfileDto>();

        // Bạn có thể thêm danh sách môn học, phân công... ở đây nếu muốn
    }
}
