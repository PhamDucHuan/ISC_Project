using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService; // ✅ Sử dụng Interface

        public GradeController(IGradeService gradeService) // ✅ Tiêm service vào
        {
            _gradeService = gradeService;
        }

        // GET: api/Grade
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGrades()
        {
            var grades = await _gradeService.GetAllGradesAsync();
            return Ok(grades);
        }

        // GET: api/Grade/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> GetGrade(int id)
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            if (grade == null)
                return NotFound($"Không tìm thấy khối với ID: {id}");

            return Ok(grade);
        }

        // POST: api/Grade
        [HttpPost]
        public async Task<ActionResult<Grade>> CreateGrade(Grade grade)
        {
            var createdGrade = await _gradeService.CreateGradeAsync(grade);
            return CreatedAtAction(nameof(GetGrade), new { id = createdGrade.GradeId }, createdGrade);
        }
    }
}
