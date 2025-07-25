using ISC_Project.DTOs.EmploymentHistory;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    [ApiController]
    [Route("api/[controller]")]
    public class EmploymentHistoryController : ControllerBase
    {
        private readonly IEmploymentHistoryService _service;
        public EmploymentHistoryController(IEmploymentHistoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmploymentHistoryDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.HistoryId }, result);
        }
    }
} 