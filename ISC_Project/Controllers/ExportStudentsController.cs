using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExportStudentsController : ControllerBase
    {
        private readonly IExportStudentService _studentService;

        public ExportStudentsController(IExportStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("export/excel/{classId}")]
        public async Task<IActionResult> ExportStudents(int classId)
        {
            try
            {
                // Nhận cả fileContents và fileName từ service
                var (fileContents, fileName) = await _studentService.ExportStudentsToExcelAsync(classId);

                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("export/school-year/{schoolYearId}")]
        public async Task<IActionResult> ExportStudentsBySchoolYear(int schoolYearId)
        {
            try
            {
                var (fileContents, fileName) = await _studentService.ExportAllClassesBySchoolYearAsync(schoolYearId);
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
