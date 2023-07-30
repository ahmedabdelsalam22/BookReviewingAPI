using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface ICountryRepository : IRepository<Country>
    {
        void UpdateAsync(Country country);
    }
}
