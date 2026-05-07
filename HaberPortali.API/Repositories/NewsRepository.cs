using HaberPortali.API.Data;
using HaberPortali.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HaberPortali.API.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _context;

        public NewsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<News>> GetAllNewsWithDetailsAsync()
        {
            return await _context.News.Include(x => x.Category).ToListAsync();
        }
        public async Task<IEnumerable<News>> GetAllAsync()
        {
            return await _context.News.ToListAsync();
        }

        public async Task<News> GetByIdAsync(int id)
        {
            return await _context.News.FindAsync(id);
        }

        public async Task AddAsync(News news)
        {
            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
        }

        public void Update(News news)
        {
            _context.News.Update(news);
            _context.SaveChanges();
        }

        public void Delete(News news)
        {
            _context.News.Remove(news);
            _context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

 
        public async Task<News> GetNewsByIdWithDetailsAsync(int id)
        {
            return await _context.News.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}