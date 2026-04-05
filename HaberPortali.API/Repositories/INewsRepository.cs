using HaberPortali.API.Models;

namespace HaberPortali.API.Repositories
{
    public interface INewsRepository : IGenericRepository<News>
    {
        // Haberi çekerken yazarını ve kategorisini de dahil eden (Include) özel metot
        Task<IEnumerable<News>> GetAllNewsWithDetailsAsync();
        Task<News?> GetNewsByIdWithDetailsAsync(int id);
    }
}