using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabGradersController : ControllerBase
    {
        private readonly ILabGraderService _service;

        public LabGradersController(ILabGraderService service)
        {
            _service = service;
        }

        [HttpGet("lab/{labScheduleId}/graders")]
        public async Task<IActionResult> GetGraders(int labScheduleId)
        {
            var users = await _service.GetGradersForLabAsync(labScheduleId);
            return Ok(users);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignGrader([FromBody] LabGrader assignment)
        {
            await _service.AssignGraderAsync(assignment.LabSchedulesId, assignment.UserId);
            return Ok("Assignment successful.");
        }
    }
}
