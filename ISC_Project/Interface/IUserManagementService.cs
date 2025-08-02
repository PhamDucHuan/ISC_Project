using ISC_Project.DTOs.UserManagement;

namespace ISC_Project.Interface
{
    public interface IUserManagementService
    {
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto?> UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserSoftAsync(int userId);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string roleName);

        Task<IEnumerable<UserDto>> SearchUsersByFullNameAsync(string fullName);
    }
}
