using ISC_Project.DTOs.SystemSetting;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingService _service;

        public SystemSettingsController(ISystemSettingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{key}")]
        public async Task<IActionResult> GetById(string key)
        {
            var item = await _service.GetByIdAsync(key);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSystemSettingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Thêm kiểm tra xem key đã tồn tại chưa
            var existingSetting = await _service.GetByIdAsync(dto.SettingKey);
            if (existingSetting != null)
            {
                return Conflict(new { message = $"Setting key '{dto.SettingKey}' đã tồn tại." });
            }

            var newItem = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { key = newItem.SettingKey }, newItem);
        }
    }
}
