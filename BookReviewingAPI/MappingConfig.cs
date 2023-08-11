using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.Auth_DTOS;
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
            CreateMap<Reviewer, ReviewerDTO>().ReverseMap();
            CreateMap<CountryCreateDTO, Country>().ReverseMap();
            CreateMap<AuthorCreateDTO, Author>();
            CreateMap<BookCreateDTO,Book>();
            CreateMap<RegisterRequestDTO,LocalUser>();
            CreateMap<ApplicationUser,UserDTO>();
        }
    }
}
       