using BookReviewingAPI.Data;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;

namespace BookReviewingAPI.Repository.IRepositoryImpl
{
    public class ReviewerRepository : Repository<Reviewer>, IReviewerRepository
    {
        private readonly ApplicationDbContext _db;
        public ReviewerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Reviewer reviewer)
        {
            _db.Reviewers.Update(reviewer);
        }
    }
}
