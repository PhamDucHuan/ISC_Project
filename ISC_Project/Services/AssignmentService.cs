// In your Services/AssignmentService.cs file
using Microsoft.EntityFrameworkCore;
using ISC_Project.Data;
using ISC_Project.Models;
using ISC_Project.DTOs;
using ISC_Project.DTOs.Assignment;
using ISC_Project.Interface;

namespace ISC_Project.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ISC_ProjectDbContext _context;

        public AssignmentService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssignmentDto>> GetAllAsync()
        {
            return await _context.Assignments
                                 .Include(a => a.Teaching) // Still need to include Teaching for its properties
                                 .Select(a => new AssignmentDto
                                 {
                                     AssignmentId = a.AssignmentId,
                                     Title = a.Title,
                                     Format = a.Format,
                                     AssignmentScope = a.AssignmentScope,
                                     Category = a.Category,
                                     StarTime = a.StarTime,
                                     EndTime = a.EndTime,
                                     AssignmentUrl = a.AssignmentUrl,
                                     Description = a.Description,
                                     Status = a.Status,
                                     PartitionType = a.PartitionType,
                                     CraeteAt = a.CraeteAt,
                                     TeachingId = a.TeachingId,
                                     TeachingAssignmentType = a.Teaching != null ? a.Teaching.AssignmentType : null, // Corrected to AssignmentType
                                     FacultyId = a.FacultyId
                                 })
                                 .ToListAsync();
        }

        public async Task<AssignmentDto?> GetByIdAsync(int id)
        {
            var assignment = await _context.Assignments
                                           .Include(a => a.Teaching)
                                           .FirstOrDefaultAsync(a => a.AssignmentId == id);
            if (assignment == null)
            {
                return null;
            }

            return new AssignmentDto
            {
                AssignmentId = assignment.AssignmentId,
                Title = assignment.Title,
                Format = assignment.Format,
                AssignmentScope = assignment.AssignmentScope,
                Category = assignment.Category,
                StarTime = assignment.StarTime,
                EndTime = assignment.EndTime,
                AssignmentUrl = assignment.AssignmentUrl,
                Description = assignment.Description,
                Status = assignment.Status,
                PartitionType = assignment.PartitionType,
                CraeteAt = assignment.CraeteAt,
                TeachingId = assignment.TeachingId,
                TeachingAssignmentType = assignment.Teaching != null ? assignment.Teaching.AssignmentType : null, // Corrected to AssignmentType
                FacultyId = assignment.FacultyId
            };
        }

        public async Task<AssignmentDto> CreateAsync(CreateAssignmentDto createDto)
        {
            var assignment = new Assignment
            {
                Title = createDto.Title,
                Format = createDto.Format,
                AssignmentScope = createDto.AssignmentScope,
                Category = createDto.Category,
                StarTime = createDto.StarTime,
                EndTime = createDto.EndTime,
                AssignmentUrl = createDto.AssignmentUrl,
                Description = createDto.Description,
                Status = createDto.Status,
                PartitionType = createDto.PartitionType,
                CraeteAt = DateTime.UtcNow,
                TeachingId = createDto.TeachingId,
                FacultyId = createDto.FacultyId
            };

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            var createdAssignment = await _context.Assignments
                                                 .Include(x => x.Teaching)
                                                 .FirstOrDefaultAsync(x => x.AssignmentId == assignment.AssignmentId);

            return new AssignmentDto
            {
                AssignmentId = createdAssignment.AssignmentId,
                Title = createdAssignment.Title,
                Format = createdAssignment.Format,
                AssignmentScope = createdAssignment.AssignmentScope,
                Category = createdAssignment.Category,
                StarTime = createdAssignment.StarTime,
                EndTime = createdAssignment.EndTime,
                AssignmentUrl = createdAssignment.AssignmentUrl,
                Description = createdAssignment.Description,
                Status = createdAssignment.Status,
                PartitionType = createdAssignment.PartitionType,
                CraeteAt = createdAssignment.CraeteAt,
                TeachingId = createdAssignment.TeachingId,
                TeachingAssignmentType = createdAssignment.Teaching != null ? createdAssignment.Teaching.AssignmentType : null, // Corrected to AssignmentType
                FacultyId = createdAssignment.FacultyId
            };
        }
    }
}