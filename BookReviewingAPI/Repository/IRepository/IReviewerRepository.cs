using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IReviewreRepository : IRepository<Reviewer>
    {
        void UpdateAsync(Reviewer reviewer);
    }
}
