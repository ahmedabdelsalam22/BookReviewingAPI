using BookReviewingAPI.Models;
using BookReviewingAPI.Models.Auth_DTOS;
using BookReviewingAPI.Repository.Auth;
using BookReviewingAPI.Repository.IRepositoryImpl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookReviewingAPI.Controllers.V1
{
    [Route("api/v{version:apiVersion}/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthRepository _userRepository;
        protected APIResponse _response;

        public UsersController(IAuthRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new APIResponse();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _userRepository.Login(loginRequestDTO);
            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { "Username or password is incorrect" };
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);

        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            bool ifUserNameIsUnique = _userRepository.IsUniqueUser(registerRequestDTO.UserName);
            if (!ifUserNameIsUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { "Username  already exists" };
                return BadRequest(_response);
            }

            var user = await _userRepository.Register(registerRequestDTO);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { "Error while registering" };
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}
