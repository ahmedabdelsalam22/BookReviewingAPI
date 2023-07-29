using System.Linq.Expressions;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>? filter = null);
        Task<T> GetAsync(int id);
        Task DeleteAsync(T entity);
        Task<T> CreateAsync(T entity);
        Task SaveChanges();
    }
}
