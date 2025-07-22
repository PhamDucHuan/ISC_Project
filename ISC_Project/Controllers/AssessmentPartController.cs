using ISC_Project.DTOs;
using ISC_Project.DTOs.AssessmentPartDto;

using ISC_Project.Models; 
using ISC_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentPartController : ControllerBase
    {
        private readonly IAssessmentPartService _service;

        public AssessmentPartController(IAssessmentPartService service) 
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssessmentPartDto createDto) // THAY ĐỔI Ở ĐÂY
        {
            // Thêm kiểm tra ModelState.IsValid để bắt lỗi validation từ Data Annotations trong DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _service.CreateAsync(createDto); // Gọi service với DTO
            return CreatedAtAction(nameof(GetById), new { id = created.AssessmentPartsId }, created);
        }

        // Bạn cũng sẽ muốn thêm các phương thức Update và Delete tương tự nếu chưa có:

        // PUT: api/AssessmentPart/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAssessmentPartDto updateDto)
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
            return NoContent();
        }

        // DELETE: api/AssessmentPart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}