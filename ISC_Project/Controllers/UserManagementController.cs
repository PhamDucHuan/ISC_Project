using ISC_Project.DTOs.UserManagement;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ISC_Project.Controllers
{
    // YÊU CẦU TẤT CẢ API TRONG ĐÂY PHẢI ĐƯỢC GỌI BỞI NGƯỜI DÙNG ĐÃ ĐĂNG NHẬP
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userService;

        public UserManagementController(IUserManagementService userService)
        {
            _userService = userService;
        }

        // CHỈ ADMIN MỚI CÓ QUYỀN TẠO USER
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _userService.GetAllRolesAsync();
            return Ok(roles);
        }

        // API NÀY BÂY GIỜ CHO PHÉP MỌI USER ĐÃ ĐĂNG NHẬP SỬ DỤNG
        [HttpGet("role/{roleName}")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var users = await _userService.GetUsersByRoleAsync(roleName);
            if (users == null || !users.Any())
            {
                return NotFound($"No users found with the role '{roleName}'.");
            }
            return Ok(users);
        }

        // API TÌM KIẾM MÀ BẠN YÊU CẦU
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string fullName)
        {
            var users = await _userService.SearchUsersByFullNameAsync(fullName);
            return Ok(users);
        }
    }
}