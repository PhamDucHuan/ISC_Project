using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomSettingController : ControllerBase
    {
        private readonly IClassroomSettingService _service;

        public ClassroomSettingController(IClassroomSettingService service)
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
            var setting = await _service.GetByIdAsync(id);
            if (setting == null) return NotFound();
            return Ok(setting);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassroomSetting setting)
        {
            var created = await _service.CreateAsync(setting);
            return CreatedAtAction(nameof(GetById), new { id = created.ClassroomSettingsId }, created);
        }
    }
}
