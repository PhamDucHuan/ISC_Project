using Microsoft.EntityFrameworkCore;
using ISC_Project.Data; 
using ISC_Project.Models;
using ISC_Project.DTOs;
using ISC_Project.DTOs.AssignmentGroup;

namespace ISC_Project.Services
{
    public class AssignmentGroupService : IAssignmentGroupService
    {
        private readonly ISC_ProjectDbContext _context;

        public AssignmentGroupService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssignmentGroupDto>> GetAllAsync()
        {
            return await _context.AssignmentGroups
                                 .Include(ag => ag.Assignments) 
                                 .Include(ag => ag.Class)      
                                 .Select(ag => new AssignmentGroupDto
                                 {
                                     AssignmentsId = ag.AssignmentsId ?? 0, 
                                     AssignmentTitle = ag.Assignments != null ? ag.Assignments.Title : null,
                                     ClassId = ag.ClassId ?? 0,            
                                     ClassName = ag.Class != null ? ag.Class.ClassName : null 
                                 })
                                 .ToListAsync();
        }

        public async Task<AssignmentGroupDto?> GetByIdAsync(int assignmentId, int classId)
        {
            // Find by composite key
            var assignmentGroup = await _context.AssignmentGroups
                                                .Include(ag => ag.Assignments)
                                                .Include(ag => ag.Class)
                                                .FirstOrDefaultAsync(ag => ag.AssignmentsId == assignmentId && ag.ClassId == classId);
            if (assignmentGroup == null)
            {
                return null;
            }

            return new AssignmentGroupDto
            {
                AssignmentsId = assignmentGroup.AssignmentsId ?? 0,
                AssignmentTitle = assignmentGroup.Assignments != null ? assignmentGroup.Assignments.Title : null,
                ClassId = assignmentGroup.ClassId ?? 0,
                ClassName = assignmentGroup.Class != null ? assignmentGroup.Class.ClassName : null
            };
        }

        public async Task<AssignmentGroupDto> CreateAsync(CreateAssignmentGroupDto createDto)
        {
            var existing = await _context.AssignmentGroups
                                         .AnyAsync(ag => ag.AssignmentsId == createDto.AssignmentsId && ag.ClassId == createDto.ClassId);
            if (existing)
            {
                throw new InvalidOperationException("This assignment is already associated with this class.");
            }

            var assignmentGroup = new AssignmentGroup
            {
                AssignmentsId = createDto.AssignmentsId,
                ClassId = createDto.ClassId
            };

            _context.AssignmentGroups.Add(assignmentGroup);
            await _context.SaveChangesAsync();

            // Fetch the created entity with related data for the DTO
            var createdAssignmentGroup = await _context.AssignmentGroups
                                                      .Include(ag => ag.Assignments)
                                                      .Include(ag => ag.Class)
                                                      .FirstOrDefaultAsync(ag => ag.AssignmentsId == assignmentGroup.AssignmentsId && ag.ClassId == assignmentGroup.ClassId);

            return new AssignmentGroupDto
            {
                AssignmentsId = createdAssignmentGroup.AssignmentsId ?? 0,
                AssignmentTitle = createdAssignmentGroup.Assignments != null ? createdAssignmentGroup.Assignments.Title : null,
                ClassId = createdAssignmentGroup.ClassId ?? 0,
                ClassName = createdAssignmentGroup.Class != null ? createdAssignmentGroup.Class.ClassName : null
            };
        }

        // Update for join table (essentially delete old and create new)
        public async Task<bool> UpdateAsync(int oldAssignmentId, int oldClassId, UpdateAssignmentGroupDto updateDto)
        {
            var existingAssignmentGroup = await _context.AssignmentGroups
                                                        .FirstOrDefaultAsync(ag => ag.AssignmentsId == oldAssignmentId && ag.ClassId == oldClassId);
            if (existingAssignmentGroup == null)
            {
                return false; // Old association not found
            }

            _context.AssignmentGroups.Remove(existingAssignmentGroup);

            var newAssociationExists = await _context.AssignmentGroups
                                                    .AnyAsync(ag => ag.AssignmentsId == updateDto.NewAssignmentsId && ag.ClassId == updateDto.NewClassId);
            if (newAssociationExists)
            {
              
                await _context.SaveChangesAsync();
                return true;
            }

            var newAssignmentGroup = new AssignmentGroup
            {
                AssignmentsId = updateDto.NewAssignmentsId,
                ClassId = updateDto.NewClassId
            };

            _context.AssignmentGroups.Add(newAssignmentGroup);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
              
                return false;
            }
        }


        public async Task<bool> DeleteAsync(int assignmentId, int classId)
        {
            var assignmentGroup = await _context.AssignmentGroups
                                                .FirstOrDefaultAsync(ag => ag.AssignmentsId == assignmentId && ag.ClassId == classId);
            if (assignmentGroup == null)
            {
                return false; // Not found
            }

            _context.AssignmentGroups.Remove(assignmentGroup);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}