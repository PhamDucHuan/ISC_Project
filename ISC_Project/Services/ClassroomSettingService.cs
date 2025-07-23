using ISC_Project.Data;
using ISC_Project.Interface;
 
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Services
{
    public class ClassroomSettingService : IClassroomSettingService
    {
        private readonly ISC_ProjectDbContext _context;

        public ClassroomSettingService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassroomSetting>> GetAllAsync()
        {
            return await _context.ClassroomSettings
                                 .Include(c => c.Class)
                                 .ToListAsync();
        }

        public async Task<ClassroomSetting?> GetByIdAsync(int id)
        {
            return await _context.ClassroomSettings
                                 .Include(c => c.Class)
                                 .FirstOrDefaultAsync(c => c.ClassroomSettingsId == id);
        }

        public async Task<ClassroomSetting> CreateAsync(ClassroomSetting setting)
        {
            _context.ClassroomSettings.Add(setting);
            await _context.SaveChangesAsync();
            return setting;
        }
    }
}
