using AutoMapper;
using BookReviewingAPI.Data;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.Auth_DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookReviewingAPI.Repository.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        private string? _secretKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            _secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower());
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            ApplicationUser? user = await _db.ApplicationUsers.FirstOrDefaultAsync(x =>
                      x.UserName!.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user!, loginRequestDTO.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    user = null,
                    Token = ""
                };
            }

            // generate token 

            var token = GenerateToken(user);


            UserDTO userDTO = _mapper.Map<UserDTO>(user);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                user = userDTO,
                Token = token,
            };
            return loginResponseDTO;
        }

        public async Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registerRequestDTO.UserName,
                Name = registerRequestDTO.Name,
                Email = registerRequestDTO.UserName,
                NormalizedEmail = registerRequestDTO.UserName.ToUpper(),
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(user, "admin");

                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registerRequestDTO.UserName);

                    return _mapper.Map<UserDTO>(userToReturn);
                }
            }
            catch (Exception ex)
            { }

            return new UserDTO();
        }

        public string GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey!);

            List<Claim> claimList = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Name,user.Name!),
            };

            var tokenDescripter = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescripter);

            return tokenHandler.WriteToken(token);
        }
    }
}
