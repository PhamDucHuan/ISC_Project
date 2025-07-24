using ISC_Project.DTOs.QuestionBank;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionBankController : ControllerBase
    {
        private readonly IQuestionBankService _service;

        public QuestionBankController(IQuestionBankService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _service.GetAllQuestionsAsync();
            return Ok(questions);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetQuestion(int questionId)
        {
            var question = await _service.GetQuestionByIdAsync(questionId);
            return question == null ? NotFound($"Không tìm thấy câu hỏi với ID {questionId}") : Ok(question);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> CreateQuestionAsync([FromBody] CreateQuestionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // In a real-world scenario, the creator's ID would be retrieved from the authentication token
            // var creatorUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var creatorUserId = 1; // Temporarily assign a sample value

            var newQuestion = await _service.CreateQuestionAsync(dto, creatorUserId);
            return CreatedAtAction(nameof(GetQuestion), new { questionId = newQuestion.QuestionId }, newQuestion);
        }
    }
}
