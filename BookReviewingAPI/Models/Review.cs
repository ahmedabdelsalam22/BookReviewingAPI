using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Attributes;

namespace BookReviewingAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string HeadLine { get; set; }
        public string ReviewText { get; set; }
        public decimal Rating { get; set; }
        public Book Book { get; set; }
        public Reviewer Reviewer { get; set; }
    }
}
