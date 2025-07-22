using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _rolePermissionService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RolePermission rolePermission)
        {
            if (rolePermission == null)
                return BadRequest("Invalid data");

            var created = await _rolePermissionService.AddAsync(rolePermission);
            return CreatedAtAction(nameof(GetAll), new { id = created.RoleId }, created);
        }
        [HttpGet("{roleId}/{permissionId}")]
        public async Task<IActionResult> GetById(int roleId, int permissionId)
        {
            var result = await _rolePermissionService.GetByIdAsync(roleId, permissionId);
            if (result == null) return NotFound("RolePermission not found");
            return Ok(result);
        }

    }
}
