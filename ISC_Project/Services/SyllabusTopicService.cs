using ISC_Project.Data;
using ISC_Project.DTOs.SyllabusTopic;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class SyllabusTopicService : ISyllabusTopicService
    {
        private readonly ISC_ProjectDbContext _context;

        public SyllabusTopicService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<SyllabusTopicDto> CreateAsync(CreateSyllabusTopicDto dto)
        {
            // Lỗi 1: Kiểm tra TeachingId có tồn tại không
            var teachingExists = await _context.TeachingAssessments.AnyAsync(t => t.TeachingId == dto.TeachingId);
            if (!teachingExists)
            {
                throw new KeyNotFoundException($"Không tìm thấy chương trình giảng dạy với ID {dto.TeachingId}.");
            }

            // Lỗi 2: Kiểm tra xem tiêu đề chủ đề đã tồn tại trong cùng một chương trình giảng dạy chưa
            var topicExists = await _context.SyllabusTopics
                .AnyAsync(st => st.TeachingId == dto.TeachingId && st.TopicTitle.ToLower() == dto.TopicTitle.ToLower());
            if (topicExists)
            {
                throw new ArgumentException($"Chủ đề '{dto.TopicTitle}' đã tồn tại trong chương trình giảng dạy này.");
            }

            var syllabusTopic = new SyllabusTopic
            {
                TopicTitle = dto.TopicTitle,
                OrderIndex = dto.OrderIndex,
                Description = dto.Description,
                TeachingId = dto.TeachingId,
                SchoolYearId = dto.SchoolYearId,
                StarTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1)
            };

            _context.SyllabusTopics.Add(syllabusTopic);
            await _context.SaveChangesAsync();

            return new SyllabusTopicDto
            {
                SyllabusId = syllabusTopic.SyllabusId,
                TopicTitle = syllabusTopic.TopicTitle,
                OrderIndex = syllabusTopic.OrderIndex,
                Description = syllabusTopic.Description,
                TeachingId = syllabusTopic.TeachingId
            };
        }

        public async Task<IEnumerable<SyllabusTopicDto>> GetAllAsync()
        {
            return await _context.SyllabusTopics.AsNoTracking().Select(st => new SyllabusTopicDto { SyllabusId = st.SyllabusId, TopicTitle = st.TopicTitle, OrderIndex = st.OrderIndex, Description = st.Description, TeachingId = st.TeachingId }).ToListAsync();
        }

        public async Task<SyllabusTopicDto?> GetByIdAsync(int id)
        {
            return await _context.SyllabusTopics.AsNoTracking().Where(st => st.SyllabusId == id).Select(st => new SyllabusTopicDto { SyllabusId = st.SyllabusId, TopicTitle = st.TopicTitle, OrderIndex = st.OrderIndex, Description = st.Description, TeachingId = st.TeachingId }).FirstOrDefaultAsync();
        }
    }
}