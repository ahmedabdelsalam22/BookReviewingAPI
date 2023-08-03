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
            CreateMap<Book, BookDTO>();
            CreateMap<Review, ReviewDTO>();
        }
    }
}
       