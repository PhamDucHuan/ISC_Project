using ISC_Project.DTOs.FacultyStudyBlock;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyStudyBlockController : ControllerBase
    {
        private readonly IFacultyStudyBlockService _service;
        public FacultyStudyBlockController(IFacultyStudyBlockService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFacultyStudyBlocks()
        {
            var facultyStudyBlocks = await _service.GetAllFacultyStudyBlocksAsync();
            return Ok(facultyStudyBlocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacultyStudyBlockById(int id)
        {
            var facultyStudyBlock = await _service.GetFacultyStudyBlockByIdAsync(id);
            if (facultyStudyBlock == null)
            {
                return NotFound(new { message = "Không tìm thấy khối học tập của khoa." });
            }
            return Ok(facultyStudyBlock);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFacultyStudyBlock([FromBody] CreateFacultyDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdBlock = await _service.CreateFacultyStudyBlockAsync(dto);
            return CreatedAtAction(nameof(GetFacultyStudyBlockById), new { id = createdBlock.FacultyId }, createdBlock);
        }
    }
}
