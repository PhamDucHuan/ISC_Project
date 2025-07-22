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

        public async Task<bool> UpdateAsync(int id, ClassroomSetting setting)
        {
            var existing = await _context.ClassroomSettings.FindAsync(id);
            if (existing == null) return false;

            existing.Status = setting.Status;
            existing.Description = setting.Description;
            existing.ClassId = setting.ClassId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.ClassroomSettings.FindAsync(id);
            if (existing == null) return false;

            _context.ClassroomSettings.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
