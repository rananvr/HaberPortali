using HaberPortali.API.Data;
using HaberPortali.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HaberPortali.API.Repositories
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<News>> GetAllNewsWithDetailsAsync()
        {
            return await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<News?> GetNewsByIdWithDetailsAsync(int id)
        {
            return await _context.News
                .Include(n => n.Category)
                .Include(n => n.Author)
                .Include(n => n.Comments)! 
                .ThenInclude(c => c.User) 
                .FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}