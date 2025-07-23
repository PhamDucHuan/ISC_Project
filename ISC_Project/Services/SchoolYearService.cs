using ISC_Project.Data;
using ISC_Project.DTOs.Class;
using ISC_Project.DTOs.School_Year;
using ISC_Project.DTOs.SchoolYear;
using ISC_Project.DTOs.Student;
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
                // ... Bước 1 & 2 không đổi ...
                // 1. TẠO NIÊN KHÓA MỚI
                var newSchoolYear = new SchoolYear
                {
                    SchoolYearName = dto.NewSchoolYearName,
                    StarTime = dto.NewStartTime,
                    EndTime = dto.NewEndTime,
                };
                _context.SchoolYears.Add(newSchoolYear);
                await _context.SaveChangesAsync();

                // 2. LẤY DỮ LIỆU CŨ
                var oldClasses = await _context.Classes
                    .Where(c => c.SchoolYearId == dto.SourceSchoolYearId)
                    .AsNoTracking().ToListAsync();

                var oldStudents = await _context.StudentsProfiles
                    .Where(s => s.SchoolYearId == dto.SourceSchoolYearId)
                    .AsNoTracking().ToListAsync();

                // === BƯỚC 3: SAO CHÉP LỚP HỌC VÀ LƯU VÀO DANH SÁCH MỚI ===
                var oldToNewClassIdMap = new Dictionary<int, int>();
                var newClassesList = new List<Class>(); // Danh sách để lưu các lớp mới

                foreach (var oldClass in oldClasses)
                {
                    var newClass = new Class
                    {
                        ClassName = oldClass.ClassName,
                        ClassCode = oldClass.ClassCode,
                        SchoolYearId = newSchoolYear.SchoolYearId,
                        // ... sao chép các thuộc tính khác
                    };
                    newClassesList.Add(newClass); // Thêm vào danh sách
                }
                await _context.Classes.AddRangeAsync(newClassesList); // Thêm hàng loạt để tối ưu
                await _context.SaveChangesAsync();

                // Tạo map ID sau khi đã lưu
                for (int i = 0; i < oldClasses.Count; i++)
                {
                    oldToNewClassIdMap[oldClasses[i].ClassId] = newClassesList[i].ClassId;
                }

                // === BƯỚC 4: SAO CHÉP HỌC VIÊN VÀ LƯU VÀO DANH SÁCH MỚI ===
                var newStudentsList = new List<StudentsProfile>(); // Danh sách để lưu học viên mới
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
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();


                // === BƯỚC 5: TẠO KẾT QUẢ TRẢ VỀ ===
                var result = new InheritedSchoolYearResultDto
                {
                    NewSchoolYear = new SchoolYearDto
                    {
                        SchoolYearId = newSchoolYear.SchoolYearId,
                        SchoolYearName = newSchoolYear.SchoolYearName,
                        StarTime = newSchoolYear.StarTime,
                        EndTime = newSchoolYear.EndTime
                    },
                    // Chuyển danh sách các model mới thành danh sách DTO
                    InheritedClasses = newClassesList.Select(c => new ClassDto
                    {
                        ClassId = c.ClassId,
                        ClassName = c.ClassName,
                        ClassCode = c.ClassCode
                    }).ToList(),
                    InheritedStudents = newStudentsList.Select(s => new StudentProfileDto
                    {
                        StudentName = s.StudentName,
                        StudentCode = s.StudentCode,
                        Status = s.Status
                    }).ToList()
                };

                return result; // Trả về đối tượng chứa đầy đủ dữ liệu
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
