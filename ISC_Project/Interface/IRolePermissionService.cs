using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<RolePermission>> GetAllAsync();
        Task<RolePermission> AddAsync(RolePermission rolePermission);
        Task<RolePermission?> GetByIdAsync(int roleId, int permissionId);
    }
}
