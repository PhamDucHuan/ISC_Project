using ISC_Project.DTOs.Exam;
using ISC_Project.Interface.Exam;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamsController(IExamService examService)
        {
            _examService = examService;
        }

        // Endpoint for teachers to create an exam
        [HttpPost]
        [Authorize(Roles = "Teacher")] // Only teachers can call this API
        public async Task<IActionResult> CreateExam([FromBody] ExamCreationDto examDto)
        {
            // Get the teacher's User ID from token
            var teacherUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var labScheduleId = await _examService.CreateRandomExamAsync(examDto, teacherUserId);
                return CreatedAtAction(nameof(GetExamById), new { id = labScheduleId }, new { labScheduleId });
            }
            catch (Exception ex)
            {
                // This line will help you trace the root cause error from the database.
                // Set a breakpoint here and inspect ex.InnerException.Message
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(errorMessage);
            }
        }

        // Endpoint for students to start the exam
        [HttpPost("{id}/start")]
        [Authorize(Roles = "Student")] // Students only
        public async Task<ActionResult<StartedExamDto>> StartExam(int id)
        {
            var studentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var startedExam = await _examService.StartExamAsync(id, studentUserId);
            return Ok(startedExam);
        }

        // Endpoint for students to submit one answer
        [HttpPost("submit-answer")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerDto answerDto)
        {
           // Additional logic should be added here to verify whether the user has permission to submit for this submission
            await _examService.SubmitAnswerAsync(answerDto);
            return Ok();
        }

        // Endpoint to finalize and grade the exam
        [HttpPost("submission/{submissionId}/finalize")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<ExamResultDto>> FinalizeExam(int submissionId)
        {
            try
            {
                var result = await _examService.FinalizeAndGradeExamAsync(submissionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Example endpoint to get exam details (not yet implemented)
        [HttpGet("{id}")]
        [Authorize(Roles = "Teacher")]
        public IActionResult GetExamById(int id)
        {
            // TODO: Implement logic to get exam details
            return Ok(new { Message = $"Details for exam {id}" });
        }
    }
}
