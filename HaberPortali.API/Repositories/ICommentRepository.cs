using HaberPortali.API.Models;

namespace HaberPortali.API.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllCommentsWithDetailsAsync();
        Task<Comment> GetByIdAsync(int id);

        Task AddAsync(Comment comment);

        void Update(Comment comment);
        void Delete(Comment comment);
        Task SaveAsync();
    }
}