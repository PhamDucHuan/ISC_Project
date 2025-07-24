using ISC_Project.DTOs.UserManagement;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/users")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userService;

        public UserManagementController(IUserManagementService userService)
        {
            _userService = userService;
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var createdUser = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetUser), new { userId = createdUser.UserId }, createdUser);
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _userService.GetAllRolesAsync();
            return Ok(roles);
        }
    }
}