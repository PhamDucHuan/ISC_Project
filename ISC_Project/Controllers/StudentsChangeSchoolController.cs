using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ISC_Project.Interface;
using ISC_Project.Models;

namespace ISC_Project.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsChangeSchoolController : ControllerBase
	{
		private readonly IStudentsChangeSchoolService _service;

		public StudentsChangeSchoolController(IStudentsChangeSchoolService service)
		{
			_service = service;
		}
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var studentChangeSchool = await _service.GetAllAsync();
            return Ok(studentChangeSchool);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var studentsChangeSchool = await _service.GetByIdAsync(id);
            return studentsChangeSchool == null ? NotFound() : Ok(studentsChangeSchool);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentsChangeSchool studentChangeSchool)
        {
            var created = await _service.CreateAsync(studentChangeSchool);
            return CreatedAtAction(nameof(GetAll), new { }, created);
        }
    }
}

