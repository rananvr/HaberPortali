using HaberPortali.API.Models;

namespace HaberPortali.API.Repositories
{
    public interface ICommentRepository
    {
        // Yorumları, hangi habere yapıldığı ve kimin yaptığı bilgisiyle beraber getirecek metot
        Task<IEnumerable<Comment>> GetAllCommentsWithDetailsAsync();
        Task<Comment> GetByIdAsync(int id);
        void Update(Comment comment);
        void Delete(Comment comment);
        Task SaveAsync();
    }
}