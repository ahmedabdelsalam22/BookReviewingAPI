using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IReviewRepository : IRepository<Review>
    {
        void Update(Review reviewer);
        void DeleteReviews(List<Review> reviews);
    }
}
