using BookReviewingAPI.Data;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;

namespace BookReviewingAPI.Repository.IRepositoryImpl
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext _db;
        public BookRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void UpdateAsync(Book book)
        {
            _db.Books.Update(book);
        }
        public List<Book> GetBooksByAuthorId(int authorId)
        {
            List<Book> books = _db.BookAuthors.Where(x => x.AuthorId == authorId).Select(x => x.Book).ToList();
            return books;
        }

    }
}
