using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IReviewRepository : IRepository<Review>
    {
        void UpdateAsync(Review reviewer);
    }
}
