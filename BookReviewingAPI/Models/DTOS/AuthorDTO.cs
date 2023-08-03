using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookReviewingAPI.Models.DTOS
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        //public Country Country { get; set; }
    }
}
