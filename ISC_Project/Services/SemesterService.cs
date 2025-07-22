using ISC_Project.Interface;
using ISC_Project.Data;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
	public class SemesterService : ISemesterService
    {
		private readonly ISC_ProjectDbContext _context;

		public SemesterService(ISC_ProjectDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Semester>> GetAllAsync()
		{
			return await _context.Semesters.ToListAsync();
		}

		public async Task<Semester?> GetByIdAsync(int id)
		{
			return await _context.Semesters.FindAsync(id);
		}

		public async Task<Semester> CreateAsync(Semester model)
		{
			_context.Semesters.Add(model);
			await _context.SaveChangesAsync();
			return model;
		}

		public async Task UpdateAsync(Semester model)
		{
			_context.Entry(model).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var entity = await _context.Semesters.FindAsync(id);
			if (entity == null) return false;
			_context.Semesters.Remove(entity);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}