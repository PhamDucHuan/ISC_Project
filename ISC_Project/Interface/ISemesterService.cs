using ISC_Project.Models;

namespace ISC_Project.Interface
{
	public interface ISemesterService
	{
		Task<IEnumerable<Semester>> GetAllAsync();
		Task<Semester?> GetByIdAsync(int id);
		Task<Semester> CreateAsync(Semester model);
		Task UpdateAsync(Semester model);
		Task<bool> DeleteAsync(int id);
	}
}
