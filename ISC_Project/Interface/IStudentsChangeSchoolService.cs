using ISC_Project.Models;

namespace ISC_Project.Interface
{
	public interface IStudentsChangeSchoolService
	{
		Task<IEnumerable<StudentsChangeSchool>> GetAllAsync();
		Task<StudentsChangeSchool?> GetByIdAsync(int id);
		Task<StudentsChangeSchool> CreateAsync(StudentsChangeSchool model);
		Task UpdateAsync(StudentsChangeSchool model);
		Task<bool> DeleteAsync(int id);
	}
}
