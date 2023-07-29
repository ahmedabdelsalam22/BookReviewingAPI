using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookReviewingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookRepository _repository;
        private APIResponse _response;

        public BookController(IBookRepository repository)
        {
            _repository = repository;
            _response = new APIResponse();
        }

        [HttpGet("allBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> GetAllBooks()
        {
            try
            {
                IEnumerable<Book> books = await _repository.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = books;
                return _response;
            }
            catch (Exception e) 
            {
                _response.ErrorMessage = new List<string> { e.ToString() };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return _response;
            }
        }


    }
}
