using ISC_Project.DTOs.RelativesInformation;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [ApiController]
    [Route("api/relatives-information")]
    [Authorize]
    public class RelativesInformationController : ControllerBase
    {
        private readonly IRelativesInformationService _service;

        public RelativesInformationController(IRelativesInformationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var relativesInformation = await _service.GetAllAsync();
            return Ok(relativesInformation);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var relativesInformation = await _service.GetByIdAsync(id);
            return relativesInformation == null ? NotFound() : Ok(relativesInformation);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRelativesInformationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newRelativesInformation = await _service.CreateAsync(dto);
            // Giả sử RelativesInformationId được tạo bởi cơ sở dữ liệu sau khi tạo
            return CreatedAtAction(nameof(GetById), new { id = newRelativesInformation.RelativesInformationId }, newRelativesInformation);
        }
    }
}