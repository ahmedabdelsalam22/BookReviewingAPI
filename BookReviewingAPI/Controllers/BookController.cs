using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

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
                _response.IsSuccess = true;
                _response.Result = books;
                return _response;
            }
            catch (Exception e)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { e.ToString() };
                return _response;
            }
        }
        [HttpGet("book/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBookById(int id) 
        {
            try
            {
                if (id == 0) 
                {
                    return BadRequest();
                }
                Book? book = await _repository.GetAsync(filter: x => x.Id == id, tracked: false);
                if (book == null) 
                {
                    return NotFound("No books exists with this id");
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = book;

                return _response;
            }
            catch (Exception e) 
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { e.ToString() };
                return _response;
            }

        }

        [HttpGet("book/isbn/{isbn}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBookByISBN(string isbn)
        {
            try
            {
                Book? book = await _repository.GetAsync(filter: x => x.Isbn == isbn, tracked: false);
                if (book == null)
                {
                    return NotFound("No books exists with this isbn");
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = book;

                return _response;
            }
            catch (Exception e)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { e.ToString() };
                return _response;
            }

        }

    }
}
