using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IAuthorRepository : IRepository<Author>
    {
        void UpdateAsync(Author author);
    }
}
