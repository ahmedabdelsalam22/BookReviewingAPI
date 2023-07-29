using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task UpdateAsync(Book book);
    }
}
