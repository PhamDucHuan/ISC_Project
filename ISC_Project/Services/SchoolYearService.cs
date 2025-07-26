using ISC_Project.Data;
using ISC_Project.DTOs.Class;
using ISC_Project.DTOs.School_Year;
using ISC_Project.DTOs.SchoolYear;
using ISC_Project.DTOs.Student;
using ISC_Project.DTOs.Subject;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
namespace ISC_Project.Services
{
    public class SchoolYearService : ISchoolYearService
    {
        private readonly ISC_ProjectDbContext _context;
        public SchoolYearService(ISC_ProjectDbContext context) { _context = context; }

        public async Task<SchoolYearDto> CreateAsync(CreateSchoolYearDto dto)
        {
            // Lưu ý: Tên model có thể cần là `SchoolYear` thay vì `School Year`
            var schoolYear = new SchoolYear
            {
                SchoolYearName = dto.SchoolYearName,
                StarTime = dto.StarTime,
                EndTime = dto.EndTime,
                SchoolId = dto.SchoolId
            };
            _context.SchoolYears.Add(schoolYear);
            await _context.SaveChangesAsync();
            return new SchoolYearDto
            {
                SchoolYearId = schoolYear.SchoolYearId,
                SchoolYearName = schoolYear.SchoolYearName,
                StarTime = (DateTime)schoolYear.StarTime,
                EndTime = (DateTime)schoolYear.EndTime
            };
        }

        public async Task<IEnumerable<SchoolYearDto>> GetAllAsync()
        {
            return await _context.SchoolYears
                .Select(sy => new SchoolYearDto
                {
                    SchoolYearId = sy.SchoolYearId,
                    SchoolYearName = sy.SchoolYearName,
                    StarTime = (DateTime)sy.StarTime,
                    EndTime = (DateTime)sy.EndTime
                })
                .ToListAsync();
        }

        public async Task<SchoolYearDto?> GetByIdAsync(int id)
        {
            return await _context.SchoolYears
                .Where(sy => sy.SchoolYearId == id)
                .Select(sy => new SchoolYearDto
                {
                    SchoolYearId = sy.SchoolYearId,
                    SchoolYearName = sy.SchoolYearName,
                    StarTime = (DateTime)sy.StarTime,
                    EndTime = (DateTime)sy.EndTime
                })
                .FirstOrDefaultAsync();
        }

        public async Task<InheritedSchoolYearResultDto> InheritAsync(InheritSchoolYearDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. TẠO NIÊN KHÓA MỚI
                var newSchoolYear = new SchoolYear
                {
                    SchoolYearName = dto.NewSchoolYearName,
                    StarTime = dto.NewStartTime,
                    EndTime = dto.NewEndTime,
                };
                _context.SchoolYears.Add(newSchoolYear);
                await _context.SaveChangesAsync();

                // 2. LẤY DỮ LIỆU CŨ TỪ NIÊN KHÓA NGUỒN
                var oldClasses = await _context.Classes
                    .Where(c => c.SchoolYearId == dto.SourceSchoolYearId)
                    .AsNoTracking().ToListAsync();

                var oldStudents = await _context.StudentsProfiles
                    .Where(s => s.SchoolYearId == dto.SourceSchoolYearId)
                    .AsNoTracking().ToListAsync();

                // ✅ LẤY DỮ LIỆU MÔN HỌC CŨ
                var oldSubjects = await _context.Subjects
                    .Where(s => s.SchoolYearId == dto.SourceSchoolYearId)
                    .AsNoTracking().ToListAsync();


                // 3. SAO CHÉP LỚP HỌC
                var oldToNewClassIdMap = new Dictionary<int, int>();
                var newClassesList = new List<Class>();

                foreach (var oldClass in oldClasses)
                {
                    var newClass = new Class
                    {
                        ClassName = oldClass.ClassName,
                        ClassCode = oldClass.ClassCode,
                        SchoolYearId = newSchoolYear.SchoolYearId,
                        // ... sao chép các thuộc tính khác
                    };
                    newClassesList.Add(newClass);
                }
                await _context.Classes.AddRangeAsync(newClassesList);
                await _context.SaveChangesAsync(); // Lưu để lấy ClassId mới

                // Tạo map ID sau khi đã lưu
                for (int i = 0; i < oldClasses.Count; i++)
                {
                    oldToNewClassIdMap[oldClasses[i].ClassId] = newClassesList[i].ClassId;
                }

                // 4. SAO CHÉP HỌC VIÊN
                var newStudentsList = new List<StudentsProfile>();
                foreach (var oldStudent in oldStudents)
                {
                    oldToNewClassIdMap.TryGetValue(oldStudent.ClassId.GetValueOrDefault(), out int newMappedClassId);

                    var newStudent = new StudentsProfile
                    {
                        StudentName = oldStudent.StudentName,
                        StudentCode = oldStudent.StudentCode,
                        Status = "Đang học",
                        SchoolYearId = newSchoolYear.SchoolYearId,
                        ClassId = newMappedClassId > 0 ? newMappedClassId : null,
                        //... sao chép các thuộc tính khác
                    };
                    newStudentsList.Add(newStudent);
                }
                await _context.StudentsProfiles.AddRangeAsync(newStudentsList);

                // ✅ 5. SAO CHÉP MÔN HỌC
                var newSubjectsList = new List<Subject>();
                foreach (var oldSubject in oldSubjects)
                {
                    var newSubject = new Subject
                    {
                        SubjectCode = oldSubject.SubjectCode,
                        SubjectsName = oldSubject.SubjectsName,
                        StarTime = oldSubject.StarTime,
                        EndTime = oldSubject.EndTime,
                        SchoolYearId = newSchoolYear.SchoolYearId, // Gán niên khóa mới
                                                                    //... sao chép các thuộc tính khác nếu có
                    };
                    newSubjectsList.Add(newSubject);
                }
                await _context.Subjects.AddRangeAsync(newSubjectsList);

                // Lưu tất cả các thay đổi vào CSDL
                await _context.SaveChangesAsync();

                // Hoàn tất giao dịch
                await transaction.CommitAsync();


                // 6. TẠO KẾT QUẢ TRẢ VỀ
                var result = new InheritedSchoolYearResultDto
                {
                    NewSchoolYear = new SchoolYearDto
                    {
                        SchoolYearId = newSchoolYear.SchoolYearId,
                        SchoolYearName = newSchoolYear.SchoolYearName,
                        StarTime = (DateTime)newSchoolYear.StarTime,
                        EndTime = (DateTime)newSchoolYear.EndTime
                    },
                    InheritedClasses = newClassesList.Select(c => new ClassDto
                    {
                        ClassId = c.ClassId,
                        ClassName = c.ClassName!,
                        ClassCode = c.ClassCode!
                    }).ToList(),
                    InheritedStudents = newStudentsList.Select(s => new StudentProfileDto
                    {
                        StudentName = s.StudentName!,
                        StudentCode = s.StudentCode!,
                        Status = s.Status!
                    }).ToList(),
                    // ✅ THÊM KẾT QUẢ MÔN HỌC
                    InheritedSubjects = newSubjectsList.Select(s => new SubjectDto
                    {
                        SubjectsId = s.SubjectsId,
                        SubjectCode = s.SubjectCode,
                        SubjectsName = s.SubjectsName
                    }).ToList()
                };

                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; // Ném lại lỗi để controller có thể xử lý
            }
        }
    }
}
