using AutoMapper;
using BookReviewingAPI.Data;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.Auth_DTOS;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookReviewingAPI.Repository.IRepositoryImpl
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        private string? _secretKey;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext db, IMapper mapper,IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _mapper = mapper;
            _secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower());
            if (user == null) 
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            LocalUser? user = await _db.LocalUsers.FirstOrDefaultAsync(x => 
                      x.UserName.ToLower() ==loginRequestDTO.UserName.ToLower() && x.Password == loginRequestDTO.Password);
            if(user == null) 
            {
                return new LoginResponseDTO()
                {
                    user = null,
                    Token = ""
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey!);

            var tokenDescripter = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescripter);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                user = user,
                Token = tokenHandler.WriteToken(token)
            };
            return loginResponseDTO;
        }

        public async Task<LocalUser> Register(RegisterRequestDTO registerRequestDTO)
        {
            LocalUser localUser = _mapper.Map<LocalUser>(registerRequestDTO);

           await _db.LocalUsers.AddAsync(localUser);
           await _db.SaveChangesAsync();

            localUser.Password = "";
           return localUser;
        }
    }
}
