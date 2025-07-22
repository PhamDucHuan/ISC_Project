using Microsoft.EntityFrameworkCore;
using ISC_Project.Data; 
using ISC_Project.Models;
using ISC_Project.DTOs;
using ISC_Project.DTOs.AssessmentPartDto; 

namespace ISC_Project.Services
{
    public class AssessmentPartService : IAssessmentPartService
    {
        private readonly ISC_ProjectDbContext _context;

        public AssessmentPartService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssessmentPartDto>> GetAllAsync()
        {
            return await _context.AssessmentParts
                                 .Include(ap => ap.Assignment) // Include Assignment if you want to show AssignmentTitle
                                 .Select(ap => new AssessmentPartDto
                                 {
                                     AssessmentPartsId = ap.AssessmentPartsId,
                                     PartOrder = ap.PartOrder,
                                     Title = ap.Title,
                                     Description = ap.Description,
                                     AssignmentUrl = ap.AssignmentUrl,
                                     StarTime = ap.StarTime,
                                     EndTime = ap.EndTime,
                                     AssignmentId = ap.AssignmentId,
                                     AssignmentTitle = ap.Assignment != null ? ap.Assignment.Title : null // Map Assignment Title
                                 })
                                 .ToListAsync();
        }

        public async Task<AssessmentPartDto?> GetByIdAsync(int id)
        {
            var ap = await _context.AssessmentParts
                                   .Include(ap => ap.Assignment)
                                   .FirstOrDefaultAsync(ap => ap.AssessmentPartsId == id);
            if (ap == null)
            {
                return null;
            }

            return new AssessmentPartDto
            {
                AssessmentPartsId = ap.AssessmentPartsId,
                PartOrder = ap.PartOrder,
                Title = ap.Title,
                Description = ap.Description,
                AssignmentUrl = ap.AssignmentUrl,
                StarTime = ap.StarTime,
                EndTime = ap.EndTime,
                AssignmentId = ap.AssignmentId,
                AssignmentTitle = ap.Assignment != null ? ap.Assignment.Title : null
            };
        }

        public async Task<AssessmentPartDto> CreateAsync(CreateAssessmentPartDto createDto)
        {
            var ap = new AssessmentPart
            {
                PartOrder = createDto.PartOrder,
                Title = createDto.Title,
                Description = createDto.Description,
                AssignmentUrl = createDto.AssignmentUrl,
                StarTime = createDto.StarTime,
                EndTime = createDto.EndTime,
                AssignmentId = createDto.AssignmentId
            };

            _context.AssessmentParts.Add(ap);
            await _context.SaveChangesAsync();

            // Fetch the Assignment to include its title in the returned DTO
            var createdAp = await _context.AssessmentParts
                                          .Include(x => x.Assignment)
                                          .FirstOrDefaultAsync(x => x.AssessmentPartsId == ap.AssessmentPartsId);


            return new AssessmentPartDto
            {
                AssessmentPartsId = createdAp.AssessmentPartsId,
                PartOrder = createdAp.PartOrder,
                Title = createdAp.Title,
                Description = createdAp.Description,
                AssignmentUrl = createdAp.AssignmentUrl,
                StarTime = createdAp.StarTime,
                EndTime = createdAp.EndTime,
                AssignmentId = createdAp.AssignmentId,
                AssignmentTitle = createdAp.Assignment != null ? createdAp.Assignment.Title : null
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateAssessmentPartDto updateDto)
        {
            var ap = await _context.AssessmentParts.FindAsync(id);
            if (ap == null)
            {
                return false; // Not found
            }

            ap.PartOrder = updateDto.PartOrder ?? ap.PartOrder; // Update if provided, otherwise keep existing
            ap.Title = updateDto.Title ?? ap.Title;
            ap.Description = updateDto.Description ?? ap.Description;
            ap.AssignmentUrl = updateDto.AssignmentUrl ?? ap.AssignmentUrl;
            ap.StarTime = updateDto.StarTime ?? ap.StarTime;
            ap.EndTime = updateDto.EndTime ?? ap.EndTime;
            ap.AssignmentId = updateDto.AssignmentId ?? ap.AssignmentId; // Be careful if you set it to null and it's not allowed in DB

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.AssessmentParts.Any(e => e.AssessmentPartsId == id))
                {
                    return false; // Not Found
                }
                else
                {
                    throw; // Other concurrency issues
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ap = await _context.AssessmentParts.FindAsync(id);
            if (ap == null)
            {
                return false; // Not found
            }

            _context.AssessmentParts.Remove(ap);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}