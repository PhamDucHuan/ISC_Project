using System.ComponentModel.DataAnnotations;

namespace ISC_Project.DTOs.Exam
{
    public class ExamCreationDto
    {
        [Required(ErrorMessage = "Tên bài kiểm tra không được để trống.")]
        public string Name { get; set; } = string.Empty!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // ✅ THAY ĐỔI QUAN TRỌNG: Chỉ cần số lượng câu hỏi
        [Required]
        [Range(1, 100, ErrorMessage = "Số lượng câu hỏi phải từ 1 đến 100.")]
        public int NumberOfQuestions { get; set; }

        // Danh sách các lớp được gán bài kiểm tra
        public List<int> ClassIds { get; set; } = new List<int>();
    }
}
