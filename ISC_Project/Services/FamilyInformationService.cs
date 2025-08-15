using DocumentFormat.OpenXml.Spreadsheet;
using ISC_Project.Data;
using ISC_Project.DTOs.Class;
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
        // Thay đổi kiểu trả về thành IEnumerable<FamilyInformationDto>
        public async Task<IEnumerable<FamilyInformationDto>> GetAllFamilyInformationAsync()
        {
            return await _context.FamilyInformations
                                 .Select(f => new FamilyInformationDto
                                 {
                                     FamilyName = f.FamilyName,
                                     BirthOfFamily = (DateTime)f.BirthOfFamily,
                                     JobFamily = f.JobFamily,
                                     PhoneFamily = f.PhoneFamily,
                                     FamilyType = f.FamilyType
                                 })
                                 .ToListAsync();
        }

        // Trong FamilyInformationService.cs
        public async Task<FamilyInformation?> GetFamilyInformationByIdAsync(int id)
        {
            return await _context.FamilyInformations
        .Where(fi => fi.UserId == id)
        .Select(fi => new FamilyInformation
        {
            FamilyName = fi.FamilyName,
            BirthOfFamily = fi.BirthOfFamily,
            JobFamily = fi.JobFamily,
            PhoneFamily = fi.PhoneFamily,
            FamilyType = fi.FamilyType,
            UserId = fi.UserId
        })
        .FirstOrDefaultAsync();
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

            // Sửa dòng này
            await _context.SaveChangesAsync(); // Dùng phiên bản async

            return entity;
        }
    }
}
