using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class ClassSubjectService : IClassSubjectService
    {
        // 1. Inject DbContext thay vì NpgsqlConnection
        private readonly ISC_ProjectDbContext _context;

        public ClassSubjectService(ISC_ProjectDbContext context) // Sửa constructor
        {
            _context = context;
        }

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            // 2. Dùng LINQ để lấy tất cả các lớp, không cần code ADO.NET dài dòng
            return await _context.Classes.ToListAsync();
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            // Tương tự, dùng LINQ cho Subjects
            return await _context.Subjects.ToListAsync();
        }

        public async Task<IEnumerable<Subject>> GetSubjectsByClassIdAsync(int classId)
        {
            // 3. Dùng LINQ để JOIN và lọc dữ liệu một cách an toàn và tường minh
            var query = from sc in _context.SubjectsClasses
                        join s in _context.Subjects on sc.SubjectsId equals s.SubjectsId
                        where sc.ClassId == classId
                        select s;

            return await query.ToListAsync();
        }

        public async Task AssignSubjectToClassAsync(int? subjectId, int? classId)
        {
            // 4. Kiểm tra đầu vào và thêm dữ liệu bằng object, không cần viết SQL
            if (subjectId == null || classId == null)
            {
                throw new ArgumentNullException("SubjectId and ClassId cannot be null.");
            }

            var subjectClass = new SubjectsClass
            {
                SubjectsId = subjectId.Value,
                ClassId = classId.Value
            };

            _context.SubjectsClasses.Add(subjectClass);
            await _context.SaveChangesAsync(); // Lưu thay đổi vào database
        }
    }
}
