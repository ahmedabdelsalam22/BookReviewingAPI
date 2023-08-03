using BookReviewingAPI.Data;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;

namespace BookReviewingAPI.Repository.IRepositoryImpl
{
    public class ReviewerRepository : Repository<Reviewer>, IReviewreRepository
    {
        private readonly ApplicationDbContext _db;
        public ReviewerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void UpdateAsync(Reviewer reviewer)
        {
            _db.Reviewers.Update(reviewer);
        }
    }
}
