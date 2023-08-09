using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.DTOS;

namespace BookReviewingAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Author, AuthorDTO>();
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Reviewer, ReviewerDTO>();
            CreateMap<CountryCreateDTO, Country>().ReverseMap();
            CreateMap<AuthorCreateDTO, Author>();
            CreateMap<BookCreateDTO,Book>();
        }
    }
}
       