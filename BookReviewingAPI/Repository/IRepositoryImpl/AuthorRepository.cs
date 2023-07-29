using BookReviewingAPI.Data;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;

namespace BookReviewingAPI.Repository.IRepositoryImpl
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly ApplicationDbContext _db;
        public AuthorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void UpdateAsync(Author author)
        {
            _db.Authors.Update(author);
        }

        public List<Author> GetAuthorByBookId(int BookId)
        {
            var author = _db.BookAuthors.Where(x=>x.BookId == BookId).Select(x=>x.Author).ToList();
            return author;
        }
    }
}
