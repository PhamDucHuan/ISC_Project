using Microsoft.AspNetCore.Mvc;
using ISC_Project.Services;
using ISC_Project.DTOs.AssessmentPartDto;
using Microsoft.AspNetCore.Authorization;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentPartsController : ControllerBase
    {
        private readonly IAssessmentPartService _service;

        public AssessmentPartsController(IAssessmentPartService service)
        {
            _service = service;
        }

        // GET: api/AssessmentParts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssessmentPartDto>>> GetAssessmentParts()
        {
            var assessmentParts = await _service.GetAllAsync();
            return Ok(assessmentParts);
        }

        // GET: api/AssessmentParts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssessmentPartDto>> GetAssessmentPart(int id)
        {
            var assessmentPart = await _service.GetByIdAsync(id);

            if (assessmentPart == null)
            {
                return NotFound();
            }

            return Ok(assessmentPart);
        }

        [HttpPost]
        public async Task<ActionResult<AssessmentPartDto>> CreateAssessmentPart(CreateAssessmentPartDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newAssessmentPart = await _service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetAssessmentPart), new { id = newAssessmentPart.AssessmentPartsId }, newAssessmentPart);
        }

    
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssessmentPart(int id, UpdateAssessmentPartDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _service.UpdateAsync(id, updateDto);

            if (!success)
            {
                return NotFound();
            }

            return NoContent(); // 204 No Content for successful update
        }

        // DELETE: api/AssessmentParts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessmentPart(int id)
        {
            var success = await _service.DeleteAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent(); // 204 No Content for successful delete
        }
    }
}