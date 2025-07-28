using ISC_Project.Controllers;
using ISC_Project.Models;

namespace ISC_Project.Interface
{
    public interface ILabGraderService
    {
        // Read
        Task<IEnumerable<User>> GetGradersForLabAsync(int labScheduleId);

        // Create
        Task AssignGraderAsync(int? labSchedulesId, int? userId);
    }
}
