using ISC_Project.Data;
using ISC_Project.DTOs.UserManagement;
using ISC_Project.Interface.AuthService;
using ISC_Project.Models;
using ISC_Project.Utils;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services.AuthService  
{
    public class AuthService : IAuthService
    {
        private readonly ISC_ProjectDbContext _context;
        private readonly IJwtService _jwtService; // ✅ Inject IJwtService
        public AuthService(ISC_ProjectDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService; // ✅
        }

        public async Task<LoginResultDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Tên đăng nhập hoặc mật khẩu không hợp lệ.");
            }

            // ✅ Call JwtService to generate token
            var accessToken = _jwtService.CreateAccessToken(user);

            var refreshToken = _jwtService.CreateRefreshToken();

            user.Token = refreshToken;
            await _context.SaveChangesAsync();

            UserDataDto userData = new UserDataDto(
                user.UserId, user.UserName, user.FullName, user.Email, user.Role.RoleName
            );

            return new LoginResultDto(accessToken, refreshToken, userData);
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(RefreshRequestDto request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Token == request.RefreshToken);

            if (user == null)
            {
                throw new ArgumentException("Refresh token không hợp lệ.");
            }

            // ✅ Call JwtService to generate new token
            var newAccessToken = _jwtService.CreateAccessToken(user);
            var newRefreshToken = _jwtService.CreateRefreshToken();

            user.Token = newRefreshToken;
            await _context.SaveChangesAsync();

            return new TokenResponseDto(newAccessToken, newRefreshToken);
        }

        public async Task<User> RegisterAsync(RegisterRequestDto request)
        {
            // ✅ Call static class to validate password
            PasswordValidator.Validate(request.Password);

            // 1. Check if the username already exists
            if (await _context.Users.AnyAsync(u => u.UserName == request.Username))
            {
                throw new ArgumentException("Tên đăng nhập đã tồn tại.");
            }

            // 2. Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Create new User object
            var newUser = new User
            {
                UserName = request.Username,
                Password = hashedPassword,
                FullName = request.FullName,
                Email = request.Email,
                RoleId = 3, // Default is Student
                Status = "Active",
                CreateAt = DateTime.UtcNow
            };

            // 4. Save to the database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

    }
}
