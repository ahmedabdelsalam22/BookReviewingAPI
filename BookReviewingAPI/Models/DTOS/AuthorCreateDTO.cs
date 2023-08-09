using BookReviewingAPI.Models.DTOS;

namespace BookReviewingAPI.Models.DTOS
{
    public class AuthorCreateDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Country Country { get; set; }
    }

}
