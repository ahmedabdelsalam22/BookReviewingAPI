using BookReviewingAPI.Data;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;

namespace BookReviewingAPI.Repository.IRepositoryImpl
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _db;
        public ReviewRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Review review)
        {
            _db.Reviews.Update(review);
        }
    }
}
