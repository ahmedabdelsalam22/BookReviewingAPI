using BookReviewingAPI.Data;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookReviewingAPI.Repository.IRepositoryImpl
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Category>> GetCategoriesByBookId(int bookId)
        {
            List<Category> categories =await _db.BookCategories.Where(x => x.BookId == bookId).Select(x=>x.Category).ToListAsync();
            return categories;
        }

        public void UpdateAsync(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
