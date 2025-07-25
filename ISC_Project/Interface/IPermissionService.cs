using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface IPermissionService
    {
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync(int permissionId);
        Task<Permission> CreateAsync(Permission newPermission);
    }
}
