using ISC_Project.Data;
using ISC_Project.DTOs.DiscussionThreads;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ISC_Project.Services
{
	public class StudentsChangeSchoolService : IStudentsChangeSchoolService
	{
		private readonly ISC_ProjectDbContext _context;

		public StudentsChangeSchoolService(ISC_ProjectDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<StudentsChangeSchool>> GetAllAsync()
		{
			return await _context.StudentsChangeSchools.ToListAsync();
		}

		public async Task<StudentsChangeSchool?> GetByIdAsync(int id)
		{
			return await _context.StudentsChangeSchools.FindAsync(id);
		}

		public async Task<StudentsChangeSchool> CreateAsync(StudentsChangeSchool model)
		{
			_context.StudentsChangeSchools.Add(model);
			await _context.SaveChangesAsync();
			return model;
		}

		public async Task UpdateAsync(StudentsChangeSchool model)
		{
			_context.Entry(model).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var entity = await _context.StudentsChangeSchools.FindAsync(id);
			if (entity == null) return false;
			_context.StudentsChangeSchools.Remove(entity);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}