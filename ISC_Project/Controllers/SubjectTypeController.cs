using ISC_Project.DTOs.SubjectType;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [ApiController]
    [Route("api/subject-types")]
    [Authorize]
    public class SubjectTypeController : ControllerBase
    {
        private readonly ISubjectTypeService _service;

        public SubjectTypeController(ISubjectTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subjectTypes = await _service.GetAllAsync();
            return Ok(subjectTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subjectType = await _service.GetByIdAsync(id);
            return subjectType == null ? NotFound() : Ok(subjectType);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubjectTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newSubjectType = await _service.CreateAsync(dto);
            // Giả sử SubjectTypeId được tạo bởi cơ sở dữ liệu sau khi tạo
            return CreatedAtAction(nameof(GetById), new { id = newSubjectType.SubjectTypeId }, newSubjectType);
        }
    }
}