using ISC_Project.Data;
using ISC_Project.DTOs.TrainingLevel;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_Project.Services
{
    public class TrainingLevelService : ITrainingLevelService
    {
        private readonly ISC_ProjectDbContext _context;

        public TrainingLevelService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<TrainingLevelDto> CreateAsync(CreateTrainingLevelDto dto)
        {
            // Lỗi 1: Kiểm tra sự tồn tại của SchoolYear nếu được cung cấp
            if (dto.SchoolYearId.HasValue)
            {
                var schoolYearExists = await _context.SchoolYears.AnyAsync(sy => sy.SchoolYearId == dto.SchoolYearId.Value);
                if (!schoolYearExists)
                {
                    throw new KeyNotFoundException($"Không tìm thấy năm học với ID {dto.SchoolYearId.Value}.");
                }
            }

            // Lỗi 2: Kiểm tra tên trình độ đào tạo đã tồn tại chưa
            var nameExists = await _context.TrainingLevels.AnyAsync(tl => tl.TrainingName.ToLower() == dto.TrainingName.ToLower());
            if (nameExists)
            {
                throw new ArgumentException($"Trình độ đào tạo '{dto.TrainingName}' đã tồn tại.");
            }

            var trainingLevel = new TrainingLevel
            {
                TrainingName = dto.TrainingName,
                TrainingForm = dto.TrainingForm,
                IsCreditBased = dto.IsCreditBased,
                DurationYears = dto.DurationYears,
                RequiredCredits = dto.RequiredCredits,
                SchoolYearId = dto.SchoolYearId,
                IsActive = true
            };

            _context.TrainingLevels.Add(trainingLevel);
            await _context.SaveChangesAsync();

            return new TrainingLevelDto
            {
                TrainingId = trainingLevel.TrainingId,
                TrainingName = trainingLevel.TrainingName,
                TrainingForm = trainingLevel.TrainingForm,
                DurationYears = trainingLevel.DurationYears
            };
        }

        public async Task<IEnumerable<TrainingLevelDto>> GetAllAsync()
        {
            return await _context.TrainingLevels.AsNoTracking().Select(tl => new TrainingLevelDto { TrainingId = tl.TrainingId, TrainingName = tl.TrainingName, TrainingForm = tl.TrainingForm, DurationYears = tl.DurationYears }).ToListAsync();
        }

        public async Task<TrainingLevelDto?> GetByIdAsync(int id)
        {
            return await _context.TrainingLevels.AsNoTracking().Where(tl => tl.TrainingId == id).Select(tl => new TrainingLevelDto { TrainingId = tl.TrainingId, TrainingName = tl.TrainingName, TrainingForm = tl.TrainingForm, DurationYears = tl.DurationYears }).FirstOrDefaultAsync();
        }
    }
}