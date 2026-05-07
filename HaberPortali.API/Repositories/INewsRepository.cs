using HaberPortali.API.Models;

namespace HaberPortali.API.Repositories
{
    public interface INewsRepository
    {
        Task<IEnumerable<News>> GetAllAsync();
        Task<News> GetByIdAsync(int id);
        Task<IEnumerable<News>> GetAllNewsWithDetailsAsync();
        Task AddAsync(News news);
        void Update(News news);
        void Delete(News news);
        Task SaveAsync();
        Task<News> GetNewsByIdWithDetailsAsync(int id);
    }
}