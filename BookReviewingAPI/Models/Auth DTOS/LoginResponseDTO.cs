namespace BookReviewingAPI.Models.Auth_DTOS
{
    public class LoginResponseDTO
    {
        public UserDTO user { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
