namespace BookReviewingAPI.Models.Auth_DTOS
{
    public class LoginResponseDTO
    {
        public LocalUser user { get; set; }
        public string Token { get; set; }
    }
}
