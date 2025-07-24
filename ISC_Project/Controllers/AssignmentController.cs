// Trong thư mục Controllers
using Microsoft.AspNetCore.Mvc;
using ISC_Project.Services;
using ISC_Project.DTOs;
using ISC_Project.DTOs.Assignment;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _service;

        public AssignmentsController(IAssignmentService service)
        {
            _service = service;
        }

        // GET: api/Assignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAssignments()
        {
            var assignments = await _service.GetAllAsync();
            return Ok(assignments);
        }

        // GET: api/Assignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentDto>> GetAssignment(int id)
        {
            var assignment = await _service.GetByIdAsync(id);

            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }

    
        [HttpPost]
        public async Task<ActionResult<AssignmentDto>> CreateAssignment(CreateAssignmentDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newAssignment = await _service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetAssignment), new { id = newAssignment.AssignmentId }, newAssignment);
        }
    }
}
