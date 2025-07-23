using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _service;

        public SemesterController(ISemesterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var semester = await _service.GetAllAsync();
            return Ok(semester);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var semester = await _service.GetByIdAsync(id);
            return semester == null ? NotFound() : Ok(semester);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Semester semester)
        {
            var created = await _service.CreateAsync(semester);
            return CreatedAtAction(nameof(GetAll), new { }, created);
        }
    }
}


