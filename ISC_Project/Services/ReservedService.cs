using ISC_Project.Data;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class ReservedService : IReservedService
    {
        private readonly ISC_ProjectDbContext _context;

        public ReservedService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reserved>> GetAllAsync()
        {
            return await _context.Reserveds.ToListAsync();
        }

        public async Task<Reserved?> GetByIdAsync(int id)
        {
            return await _context.Reserveds.FindAsync(id);
        }

        public async Task<Reserved> CreateAsync(Reserved model)
        {
            _context.Reserveds.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task UpdateAsync(Reserved model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}