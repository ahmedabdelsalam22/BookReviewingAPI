using System.Net;

namespace BookReviewingAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string>? ErrorMessage { get; set; }
        public object? Result { get; set; }
    }
}
