using ISC_Project.Data;
using ISC_Project.DTOs.Score;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class ScoreService : IScoreService
    {
        private readonly ISC_ProjectDbContext _context;

        public ScoreService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Score>> GetAllAsync()
        {
            return await _context.Scores.Include(s => s.Subjects).ToListAsync();
        }

        public async Task<Score?> GetByIdAsync(int id)
        {
            // Dùng FirstOrDefaultAsync để trả về null nếu không tìm thấy
            return await _context.Scores
                .Include(s => s.Subjects) // Lấy kèm thông tin môn học
                .FirstOrDefaultAsync(s => s.ScoreId == id);
        }

        public async Task<Score> CreateAsync(CreateScoreDto scoreDto)
        {
            // 1. Tạo một đối tượng Score mới từ DTO
            var newScore = new Score
            {
                // ScoreId sẽ được database tự động tạo, không cần gán ở đây
                ScoreType = scoreDto.ScoreType,
                Coefficient = scoreDto.Coefficient,
                ScoreNumber = scoreDto.ScoreNumber,
                AverageScore = scoreDto.AverageScore,
                Semester = scoreDto.Semester,
                SubjectsId = scoreDto.SubjectsId,
                ClassId = scoreDto.ClassId,
                SchoolYearId = scoreDto.SchoolYearId
            };

            // 2. Thêm đối tượng Score mới vào DbContext và lưu lại
            _context.Scores.Add(newScore);
            await _context.SaveChangesAsync();

            // 3. Trả về đối tượng Score đã được tạo (bây giờ sẽ có ScoreId)
            return newScore;
        }
    }
}
