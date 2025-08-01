using ISC_Project.Data;
using ISC_Project.Interface;
using ISC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_Project.Services
{
    public class ScoreService : IScoreService
    {
        private readonly ISC_ProjectDbContext _context;

        public ScoreService(ISC_ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Score>> GetAllAsync()
        {
            return await _context.Scores.Include(s => s.Subjects).ToListAsync();
        }

        public async Task<Score> CreateAsync(Score score)
        {
            _context.Scores.Add(score);
            await _context.SaveChangesAsync();
            return score;
        }
    }
}
