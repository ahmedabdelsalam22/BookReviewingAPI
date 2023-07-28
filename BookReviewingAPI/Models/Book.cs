namespace BookReviewingAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public DateTime DatePublished { get; set; }
        public List<Review> Reviews { get; set; }
        public List<BookCategory> BookCategories { get; set; }

    }
}
