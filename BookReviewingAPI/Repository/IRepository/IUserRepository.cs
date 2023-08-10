using BookReviewingAPI.Models;
using BookReviewingAPI.Models.Auth_DTOS;

namespace BookReviewingAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegisterRequestDTO registerRequestDTO);
    }
}
