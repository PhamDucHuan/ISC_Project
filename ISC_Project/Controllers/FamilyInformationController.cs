using ISC_Project.DTOs.FamilyInformation;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyInformationController : ControllerBase
    {
        private readonly IFamilyInformationService _service;
        public FamilyInformationController(IFamilyInformationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllFamilyInformation()
        {
            var familyInformation = await _service.GetAllFamilyInformationAsync();
            return Ok(familyInformation);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public async Task<IActionResult> GetFamilyInformationById(int id)
        {
            var familyInformation = await _service.GetFamilyInformationByIdAsync(id);
            if (familyInformation == null)
            {
                return NotFound(new { message = "Không tìm thấy thông tin gia đình." });
            }
            return Ok(familyInformation);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> CreateFamilyInformation([FromBody] CreateFamilyInformationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdFamilyInfo = await _service.CreateFamilyInformationAsync(dto);
            return CreatedAtAction(nameof(GetFamilyInformationById), new { id = createdFamilyInfo.FamilyName }, createdFamilyInfo);
        }
    }
}
