using ISC_Project.DTOs.Score;
using ISC_Project.Interface;
using ISC_Project.Models;
using ISC_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreService _service;

        public ScoreController(IScoreService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var scores = await _service.GetAllAsync();
            return Ok(scores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Score>> GetScoreById(int id)
        {
            var score = await _service.GetByIdAsync(id);
            if (score == null)
            {
                return NotFound(); // Trả về 404 Not Found nếu không có score
            }
            return Ok(score);
        }

        [HttpPost]
        public async Task<ActionResult<Score>> CreateScore(CreateScoreDto scoreDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdScore = await _service.CreateAsync(scoreDto);

            // Trả về đối tượng Score đã được tạo thành công
            return CreatedAtAction(nameof(GetScoreById), new { id = createdScore.ScoreId }, createdScore);
        }
    }
}
