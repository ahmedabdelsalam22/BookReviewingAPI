using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        void UpdateAsync(Book book);
        public List<Book> GetBooksByAuthorId(int authorId);
    }
}
