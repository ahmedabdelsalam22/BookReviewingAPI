using BookReviewingAPI.Repository.IRepository;

namespace BookReviewingAPI.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAuthorRepository authorRepository { get; }
        IBookRepository bookRepository { get; }
        ICategoryRepository categoryRepository { get; }
        ICountryRepository countryRepository { get; }
        IReviewerRepository reviewerRepository { get; }
        IReviewRepository reviewRepository { get; }
        Task SaveChangesAsync();
    }
}
