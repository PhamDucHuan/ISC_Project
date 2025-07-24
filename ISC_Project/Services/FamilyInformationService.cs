using ISC_Project.Data;
using ISC_Project.DTOs.FamilyInformation;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class FamilyInformationService : IFamilyInformationService
    {
        private readonly ISC_ProjectDbContext _context;
        public FamilyInformationService(ISC_ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<IFamilyInformationService>> GetAllFamilyInformationAsync()
        {
            return (IEnumerable<IFamilyInformationService>)await _context.FamilyInformations.ToListAsync();
        }
        public async Task<FamilyInformation?> GetFamilyInformationByIdAsync(int id)
        {
            return await  _context.FamilyInformations.FindAsync(id).AsTask();
        }
        public async Task<FamilyInformation> CreateFamilyInformationAsync(CreateFamilyInformationDto dto)
        {
            var entity = new FamilyInformation
            {
                FamilyName = dto.FamilyName,
                BirthOfFamily = dto.BirthOfFamily,
                JobFamily = dto.JobFamily,
                PhoneFamily = dto.PhoneFamily,
                FamilyType = dto.FamilyType
            };
            _context.FamilyInformations.Add(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
