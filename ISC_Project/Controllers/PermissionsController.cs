using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        // GET: api/Permissions
        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _permissionService.GetAllAsync();
            return Ok(permissions);
        }

        // GET: api/Permissions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            var permission = await _permissionService.GetByIdAsync(id);
            if (permission == null)
            {
                return NotFound();
            }
            return Ok(permission);
        }

        // POST: api/Permissions
        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] Permission newPermission)
        {
            if (newPermission == null)
            {
                return BadRequest();
            }

            var createdPermission = await _permissionService.CreateAsync(newPermission);
            return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.PermissionsId }, createdPermission);
        }
    }
}
