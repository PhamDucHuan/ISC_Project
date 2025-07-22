using Microsoft.AspNetCore.Mvc;
using ISC_Project.Services;
using ISC_Project.DTOs;
using System;
using ISC_Project.DTOs.AssignmentGroup;
using Microsoft.AspNetCore.Authorization;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentGroupsController : ControllerBase
    {
        private readonly IAssignmentGroupService _service;

        public AssignmentGroupsController(IAssignmentGroupService service)
        {
            _service = service;
        }

        // GET: api/AssignmentGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentGroupDto>>> GetAssignmentGroups()
        {
            var assignmentGroups = await _service.GetAllAsync();
            return Ok(assignmentGroups);
        }

        // GET: api/AssignmentGroups/{assignmentId}/{classId}
        [HttpGet("{assignmentId}/{classId}")]
        public async Task<ActionResult<AssignmentGroupDto>> GetAssignmentGroup(int assignmentId, int classId)
        {
            var assignmentGroup = await _service.GetByIdAsync(assignmentId, classId);

            if (assignmentGroup == null)
            {
                return NotFound();
            }

            return Ok(assignmentGroup);
        }

      
        [HttpPost]
        public async Task<ActionResult<AssignmentGroupDto>> CreateAssignmentGroup(CreateAssignmentGroupDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newAssignmentGroup = await _service.CreateAsync(createDto);
                // For composite key, CreatedAtAction can be tricky. You might simplify it.
                return CreatedAtAction(nameof(GetAssignmentGroup), new { assignmentId = newAssignmentGroup.AssignmentsId, classId = newAssignmentGroup.ClassId }, newAssignmentGroup);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409 Conflict if association already exists
            }
            catch (Exception ex) // Catch other potential errors like FK violations
            {
                return BadRequest(new { message = "Error creating assignment group: " + ex.Message });
            }
        }
    }
}