﻿using ISC_Project.DTOs.UserManagement;
using ISC_Project.Interface.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace ISC_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var loginResult = await _authService.LoginAsync(request);

                // ✅ CREATE FINAL RESPONSE OBJECT AS REQUESTED
                var response = new
                {
                    message = "Đăng nhập thành công",
                    data = loginResult.UserData, // User data
                    tokens = new // Token data
                    {
                        accessToken = loginResult.AccessToken,
                        refreshToken = loginResult.RefreshToken
                    }
                };

                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto request)
        {
            try
            {
                var tokens = await _authService.RefreshTokenAsync(request);
                return Ok(tokens);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                var newUser = await _authService.RegisterAsync(request);

                // ✅ CREATE RESPONSE OBJECT AS REQUESTED
                var response = new
                {
                    message = "Đăng ký thành công", // Success message
                    data = new // Returned user data
                    {
                        UserId = newUser.UserId,
                        Username = newUser.UserName,
                        FullName = newUser.FullName,
                        Email = newUser.Email
                        // You can add other fields from newUser here if needed
                    }
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"INNER EXCEPTION: {ex.InnerException.Message}");
                }
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình đăng ký." });
            }
        }
    }
}
