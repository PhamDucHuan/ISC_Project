using ISC_Project.DTOs.Exemption;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/exemptions")]
    public class ExemptionsController : ControllerBase
    {
        private readonly IExemptionService _service;

        public ExemptionsController(IExemptionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exemption = await _service.GetByIdAsync(id);
            return exemption == null ? NotFound() : Ok(exemption);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExemptionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var exemption = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = exemption.ExemptionsId }, exemption);
        }

       
    }
}