using ClosedXML.Excel;
using ISC_Project.Data;
using ISC_Project.Interface;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class ExportStudentService : IExportStudentService
    {
        private readonly ISC_ProjectDbContext _context;

        public ExportStudentService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<(byte[] fileContents, string fileName)> ExportStudentsToExcelAsync(int classId)
        {
            var classInfo = await _context.Classes.FindAsync(classId);
            if (classInfo == null)
            {
                throw new Exception($"Không tìm thấy lớp học với ID {classId}.");
            }

            var students = await _context.StudentsProfiles
                                         .Where(s => s.ClassId == classId)
                                         .OrderBy(s => s.StudentName)
                                         .ToListAsync();

            // Lấy thông tin niên khóa
            string schoolYearName = "ChuaXacDinh";
            if (students.Any())
            {
                var schoolYearId = students.First().SchoolYearId;
                if (schoolYearId.HasValue)
                {
                    var schoolYear = await _context.SchoolYears.FindAsync(schoolYearId.Value);
                    if (schoolYear != null && !string.IsNullOrEmpty(schoolYear.SchoolYearName))
                    {
                        // Thay thế các ký tự không hợp lệ trong tên file
                        schoolYearName = schoolYear.SchoolYearName.Replace("/", "-");
                    }
                }
            }

            // Tạo tên file
            string fileName = $"DanhSachHocSinh_Lop_{classInfo.ClassName}-{schoolYearName}.xlsx";


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Danh sách học sinh");

                worksheet.Cell("A1").Value = $"Danh sách học sinh lớp: {classInfo.ClassName}";
                worksheet.Cell("A1").Style.Font.Bold = true;
                worksheet.Cell("A1").Style.Font.FontSize = 16;
                worksheet.Range("A1:E1").Merge();

                // ... (Phần còn lại của code tạo tiêu đề và điền dữ liệu vẫn giữ nguyên)
                worksheet.Cell("A3").Value = "Mã học sinh";
                worksheet.Cell("B3").Value = "Họ và tên";
                worksheet.Cell("C3").Value = "Ngày sinh";
                worksheet.Cell("D3").Value = "Giới tính";

                var headerRange = worksheet.Range("A3:D3");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                if (students.Any())
                {
                    var currentRow = 4;
                    foreach (var student in students)
                    {
                        worksheet.Cell(currentRow, 1).Value = student.StudentCode;
                        worksheet.Cell(currentRow, 2).Value = student.StudentName;
                        worksheet.Cell(currentRow, 3).Value = student.DateOfBirth?.ToString("dd/MM/yyyy") ?? "N/A";
                        worksheet.Cell(currentRow, 4).Value = student.Sex;
                        currentRow++;
                    }
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    // Trả về cả nội dung file và tên file
                    return (stream.ToArray(), fileName);
                }
            }
        }

        public async Task<(byte[] fileContents, string fileName)> ExportAllClassesBySchoolYearAsync(int schoolYearId)
        {
            var schoolYear = await _context.SchoolYears.FindAsync(schoolYearId);
            if (schoolYear == null)
            {
                throw new Exception($"Không tìm thấy niên khóa với ID {schoolYearId}.");
            }

            var classesInYear = await _context.Classes
                                              .Where(c => c.SchoolYearId == schoolYearId)
                                              .OrderBy(c => c.ClassName)
                                              .ToListAsync();

            if (!classesInYear.Any())
            {
                throw new Exception($"Không có lớp học nào trong niên khóa '{schoolYear.SchoolYearName}'.");
            }

            var sanitizedSchoolYearName = schoolYear.SchoolYearName?.Replace("/", "-") ?? "KhongTen";
            var fileName = $"DanhSachCacLop_NienKhoa_{sanitizedSchoolYearName}.xlsx";

            using (var workbook = new XLWorkbook())
            {
                foreach (var cls in classesInYear)
                {
                    // Tên sheet không được chứa ký tự đặc biệt và không quá 31 ký tự
                    var sheetName = new string(cls.ClassName.Where(char.IsLetterOrDigit).ToArray());
                    if (string.IsNullOrWhiteSpace(sheetName))
                    {
                        sheetName = $"Lop_{cls.ClassId}";
                    }
                    sheetName = sheetName.Length > 31 ? sheetName.Substring(0, 31) : sheetName;

                    var worksheet = workbook.Worksheets.Add(sheetName);

                    worksheet.Cell("A1").Value = $"DANH SÁCH HỌC SINH LỚP: {cls.ClassName?.ToUpper()}";
                    worksheet.Cell("A1").Style.Font.Bold = true;
                    worksheet.Cell("A1").Style.Font.FontSize = 14;
                    worksheet.Range("A1:D1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    // Thêm tiêu đề cột
                    worksheet.Cell("A3").Value = "Mã học sinh";
                    worksheet.Cell("B3").Value = "Họ và tên";
                    worksheet.Cell("C3").Value = "Ngày sinh";
                    worksheet.Cell("D3").Value = "Giới tính";

                    var headerRange = worksheet.Range("A3:D3");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                    // Lấy danh sách học sinh của lớp hiện tại
                    var students = await _context.StudentsProfiles
                                                 .Where(s => s.ClassId == cls.ClassId)
                                                 .OrderBy(s => s.StudentName)
                                                 .ToListAsync();

                    if (students.Any())
                    {
                        var currentRow = 4;
                        foreach (var student in students)
                        {
                            worksheet.Cell(currentRow, 1).Value = student.StudentCode;
                            worksheet.Cell(currentRow, 2).Value = student.StudentName;
                            worksheet.Cell(currentRow, 3).Value = student.DateOfBirth?.ToString("dd/MM/yyyy");
                            worksheet.Cell(currentRow, 4).Value = student.Sex;
                            currentRow++;
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return (stream.ToArray(), fileName);
                }
            }
        }
    }
}
