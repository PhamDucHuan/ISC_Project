using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ISC_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsChangeClassController : ControllerBase
    {
        private readonly IStudentsChangeClassService _studentsChangeClassService;

        public StudentsChangeClassController(IStudentsChangeClassService studentsChangeClassService)
        {
            _studentsChangeClassService = studentsChangeClassService;
        }

        // GET: api/StudentsChangeClass
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studentsChangeClassService.GetAllAsync();
            return Ok(result);
        }

        // POST: api/StudentsChangeClass
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentsChangeClass changeRequest)
        {
            if (changeRequest == null)
                return BadRequest("Invalid data");

            var created = await _studentsChangeClassService.AddAsync(changeRequest);
            return CreatedAtAction(nameof(GetAll), new { id = created.StudentsChangeClassesId }, created);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _studentsChangeClassService.GetByIdAsync(id);
            if (result == null) return NotFound("StudentsChangeClass not found");
            return Ok(result);
        }

    }
}
