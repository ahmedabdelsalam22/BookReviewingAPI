using BookReviewingAPI.Models;
using BookReviewingAPI.Models.DTOS;
using BookReviewingAPI.Repository.IRepositoryImpl;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface ICountryRepository : IRepository<Country>
    {
        void Update(Country country);
    }
}
