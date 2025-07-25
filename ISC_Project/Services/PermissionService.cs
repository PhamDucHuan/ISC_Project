using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly ISC_ProjectDbContext _context;

        public PermissionService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<Permission?> GetByIdAsync(int permissionId)
        {
            return await _context.Permissions.FindAsync(permissionId);
        }

        public async Task<Permission> CreateAsync(Permission newPermission)
        {
            _context.Permissions.Add(newPermission);
            await _context.SaveChangesAsync();
            return newPermission;
        }
    }
}
