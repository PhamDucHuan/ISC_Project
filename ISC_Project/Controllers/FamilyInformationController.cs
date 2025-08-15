using ISC_Project.DTOs.FamilyInformation;
using ISC_Project.Interface;
using ISC_Project.Services;
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
            // 1. Gọi service để lấy dữ liệu
            var familyInfo = await _service.GetFamilyInformationByIdAsync(id);

            // 2. KIỂM TRA NULL (Bước quan trọng nhất để sửa lỗi)
            if (familyInfo == null)
            {
                // Nếu không tìm thấy, trả về lỗi 404 Not Found cho client
                return NotFound($"Không tìm thấy thông tin gia đình với ID = {id}");
            }

            // 3. Nếu đối tượng tồn tại (không phải null), tiếp tục xử lý và trả về 200 OK
            return Ok(familyInfo);
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
