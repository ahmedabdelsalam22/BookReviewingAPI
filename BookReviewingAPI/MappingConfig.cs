using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.DTOS;
using BookReviewingAPI.Models.DTOS.Country;

namespace BookReviewingAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Author, AuthorDTO>();
            CreateMap<Book, BookDTO>();
            CreateMap<Review, ReviewDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Country, CountryDTO>();
            CreateMap<Reviewer, ReviewerDTO>();
            CreateMap<CountryCreateDTO, Country>();
        }
    }
}
       