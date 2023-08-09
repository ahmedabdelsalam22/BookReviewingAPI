using BookReviewingAPI.Data;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookReviewingAPI.Repository.IRepositoryImpl
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly ApplicationDbContext _db;
        public BookRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Book book)
        {
            _db.Books.Update(book);
        }
        public async Task<List<Book>> GetBooksByAuthorId(int authorId)
        {
            List<Book> books =await _db.BookAuthors.Where(x => x.AuthorId == authorId).Select(x => x.Book).ToListAsync();
            return books;
        }

        public async Task<List<Book>> GetBooksByCategoryId(int categoryId)
        {
            List<Book> books = await _db.BookCategories.Where(x => x.CategoryId == categoryId).Select(x => x.Book).ToListAsync();
            return books;
        }
    }
}
