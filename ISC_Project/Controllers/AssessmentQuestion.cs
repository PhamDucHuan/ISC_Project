using Microsoft.AspNetCore.Mvc;
using ISC_Project.Services;
using ISC_Project.DTOs;
using ISC_Project.DTOs.AssessmentQuestion.ISC_Project.DTOs;
using ISC_Project.DTOs.AssessmentQuestion;
using Microsoft.AspNetCore.Authorization;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentQuestionsController : ControllerBase
    {
        private readonly IAssessmentQuestionService _service;

        public AssessmentQuestionsController(IAssessmentQuestionService service)
        {
            _service = service;
        }

        // GET: api/AssessmentQuestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssessmentQuestionDto>>> GetAssessmentQuestions()
        {
            var assessmentQuestions = await _service.GetAllAsync();
            return Ok(assessmentQuestions);
        }

        // GET: api/AssessmentQuestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssessmentQuestionDto>> GetAssessmentQuestion(int id)
        {
            var assessmentQuestion = await _service.GetByIdAsync(id);

            if (assessmentQuestion == null)
            {
                return NotFound();
            }

            return Ok(assessmentQuestion);
        }

       
        [HttpPost]
        public async Task<ActionResult<AssessmentQuestionDto>> CreateAssessmentQuestion(CreateAssessmentQuestionDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newAssessmentQuestion = await _service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetAssessmentQuestion), new { id = newAssessmentQuestion.AssessmentQuestionsId }, newAssessmentQuestion);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssessmentQuestion(int id, UpdateAssessmentQuestionDto updateDto)
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

        // DELETE: api/AssessmentQuestions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessmentQuestion(int id)
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