using ISC_Project.Data;
using ISC_Project.DTOs.Campus;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Services
{
    public class CampusService : ICampusService
    {
        private readonly ISC_ProjectDbContext _context;
        public CampusService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<CampusDto> CreateAsync(CreateCampusDto dto)
        {
            // Lỗi 1: Kiểm tra sự tồn tại của SchoolId
            var schoolExists = await _context.SchoolProfiles.AnyAsync(s => s.SchoolId == dto.SchoolId);
            if (!schoolExists)
            {
                throw new KeyNotFoundException($"Không tìm thấy trường học với ID {dto.SchoolId}.");
            }

            // Lỗi 2: Kiểm tra tên cơ sở có bị trùng trong cùng một trường không
            var campusNameExists = await _context.Campuses
                .AnyAsync(c => c.SchoolId == dto.SchoolId && c.Name.ToLower() == dto.Name.ToLower());
            if (campusNameExists)
            {
                throw new ArgumentException($"Tên cơ sở '{dto.Name}' đã tồn tại trong trường này.");
            }

            var campus = new Campus
            {
                Name = dto.Name,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                SchoolId = dto.SchoolId
            };

            _context.Campuses.Add(campus);
            await _context.SaveChangesAsync();

            return new CampusDto
            {
                CampusesId = campus.CampusesId,
                Name = campus.Name,
                Address = campus.Address,
                PhoneNumber = campus.PhoneNumber,
                Email = campus.Email
            };
        }

        public async Task<IEnumerable<CampusDto>> GetAllAsync()
        {
            return await _context.Campuses
                .AsNoTracking()
                .Select(c => new CampusDto
                {
                    CampusesId = c.CampusesId,
                    Name = c.Name,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email
                }).ToListAsync();
        }

        public async Task<CampusDto?> GetByIdAsync(int id)
        {
            return await _context.Campuses
                .AsNoTracking()
                .Where(c => c.CampusesId == id)
                .Select(c => new CampusDto
                {
                    CampusesId = c.CampusesId,
                    Name = c.Name,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email
                }).FirstOrDefaultAsync();
        }
    }
}