using System.Linq.Expressions;

namespace HaberPortali.API.Repositories
{
    // "T" burada herhangi bir tablomuzu (News, Category vb.) temsil ediyor.
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}