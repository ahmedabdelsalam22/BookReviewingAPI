using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        void Update(Book book);
        public Task<List<Book>> GetBooksByAuthorId(int authorId);
        public Task<List<Book>> GetBooksByCategoryId(int categoryId);

    }
}
