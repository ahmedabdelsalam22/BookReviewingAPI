using BookReviewingAPI.Models;
using BookReviewingAPI.Models.Auth_DTOS;

namespace BookReviewingAPI.Repository.Auth
{
    public interface IAuthRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);
        string GenerateToken(ApplicationUser user);
    }
}
