using BookReviewingAPI.Models;
using BookReviewingAPI.Models.Auth_DTOS;

namespace BookReviewingAPI.Repository.Auth
{
    public interface IAuthRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);
        Task<string> GenerateToken(ApplicationUser user);
        Task<bool> AssignRole(string email, string roleName);

    }
}
