using System.Linq.Expressions;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T,bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null,bool tracked = true);
        void Delete(T entity);
        Task CreateAsync(T entity);
        Task SaveChanges();
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>>? filter = null);
    }
}
