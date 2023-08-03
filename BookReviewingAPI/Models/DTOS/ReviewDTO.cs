using System.ComponentModel.DataAnnotations;

namespace BookReviewingAPI.Models.DTOS
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string HeadLine { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
    }
}
