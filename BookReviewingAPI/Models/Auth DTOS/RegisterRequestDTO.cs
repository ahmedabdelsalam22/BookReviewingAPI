namespace BookReviewingAPI.Models.Auth_DTOS
{
    public class RegisterRequestDTO
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
