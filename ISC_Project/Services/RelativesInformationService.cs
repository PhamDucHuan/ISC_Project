using ISC_Project.Data;
using ISC_Project.DTOs.RelativesInformation;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class RelativesInformationService : IRelativesInformationService
    {
        private readonly ISC_ProjectDbContext _context;

        public RelativesInformationService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<RelativesInformationDto?> GetByIdAsync(int relativesInformationId)
        {
            return await _context.RelativesInformations
                .AsNoTracking()
                .Where(r => r.RelativesInformationId == relativesInformationId)
                .Select(r => new RelativesInformationDto
                {
                    RelativesInformationId = r.RelativesInformationId,
                    RelativesName = r.RelativesName,
                    Address = r.Address,
                    PhoneNumber = r.PhoneNumber,
                    RegistrationsId = r.RegistrationsId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RelativesInformationDto>> GetAllAsync()
        {
            return await _context.RelativesInformations
                .AsNoTracking()
                .Select(r => new RelativesInformationDto
                {
                    RelativesInformationId = r.RelativesInformationId,
                    RelativesName = r.RelativesName,
                    Address = r.Address,
                    PhoneNumber = r.PhoneNumber,
                    RegistrationsId = r.RegistrationsId
                })
                .ToListAsync();
        }

        public async Task<RelativesInformation> CreateAsync(CreateRelativesInformationDto dto)
        {
            var newRelativesInformation = new RelativesInformation
            {
                RelativesName = dto.RelativesName,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                RegistrationsId = dto.RegistrationsId
            };

            _context.RelativesInformations.Add(newRelativesInformation);
            await _context.SaveChangesAsync();
            return newRelativesInformation;
        }
    }
}