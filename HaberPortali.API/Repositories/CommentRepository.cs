using HaberPortali.API.Data;
using HaberPortali.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HaberPortali.API.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context; // Kendi DbContext ismin farklıysa burayı değiştir

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsWithDetailsAsync()
        {
            // Yorumları çekerken Haberi ve Kullanıcıyı da dahil ediyoruz (.Include)
            return await _context.Comments
                .Include(c => c.News)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt) // En yeni yorum en üstte çıksın
                .ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
        }

        public void Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}