using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly ISC_ProjectDbContext _context;

        public RolePermissionService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolePermission>> GetAllAsync()
        {
            return await _context.RolePermissions
                                 .Include(rp => rp.Role)
                                 .Include(rp => rp.Permissions)
                                 .ToListAsync();
        }

        public async Task<RolePermission> AddAsync(RolePermission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();
            return rolePermission;
        }
        public async Task<RolePermission?> GetByIdAsync(int roleId, int permissionId)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Role)
                .Include(rp => rp.Permissions)
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionsId == permissionId);
        }

    }
}
