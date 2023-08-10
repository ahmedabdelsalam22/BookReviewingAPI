using BookReviewingAPI.Data;
using BookReviewingAPI.Repository.IRepository;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            authorRepository = new AuthorRepository(_db);
            bookRepository = new BookRepository(_db);
            categoryRepository = new CategoryRepository(_db);
            countryRepository = new CountryRepository(_db);
            reviewerRepository = new ReviewerRepository(_db);
            reviewRepository = new ReviewRepository(_db);
        }

        public IAuthorRepository authorRepository { get; private set; }
        public IBookRepository bookRepository { get; private set; }
        public ICategoryRepository categoryRepository { get; private set; }
        public ICountryRepository countryRepository { get; private set; }
        public IReviewerRepository reviewerRepository { get; private set; }
        public IReviewRepository reviewRepository { get; private set; }


        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
