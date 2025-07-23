using ISC_Project.Interface;
using ISC_Project.Models;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Score score)
        {
            var created = await _service.CreateAsync(score);
            return CreatedAtAction(nameof(GetAll), new { }, created);
        }
    }
}
