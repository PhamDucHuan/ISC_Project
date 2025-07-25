using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PastClassesController : ControllerBase
    {
        private readonly IPastClassesService _pastClassService;

        public PastClassesController(IPastClassesService pastClassService)
        {
            _pastClassService = pastClassService;
        }

        // GET: api/PastClasses
        [HttpGet]
        public async Task<IActionResult> GetAllPastClasses()
        {
            var classes = await _pastClassService.GetAllAsync();
            return Ok(classes);
        }

        // GET: api/PastClasses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPastClassById(int id)
        {
            var pastClass = await _pastClassService.GetByIdAsync(id);
            if (pastClass == null)
            {
                return NotFound();
            }
            return Ok(pastClass);
        }

        // POST: api/PastClasses
        [HttpPost]
        public async Task<IActionResult> CreatePastClass([FromBody] PastClass newClass)
        {
            if (newClass == null)
            {
                return BadRequest();
            }

            var createdClass = await _pastClassService.CreateAsync(newClass);

            // Trả về response 201 Created cùng với đối tượng vừa tạo
            return CreatedAtAction(nameof(GetPastClassById), new { id = createdClass.ClassId }, createdClass);
        }
    }
}
