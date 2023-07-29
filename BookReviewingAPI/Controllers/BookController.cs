using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookReviewingAPI.Controllers
{
    [Route("api/")]
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
        [HttpGet("book/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBookById(int bookId) 
        {
            try
            {
                if (bookId == 0) 
                {
                    return BadRequest();
                }
                Book? book = await _repository.GetAsync(filter: x => x.Id == bookId, tracked: false);
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

        [HttpGet("book/{bookId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRatingByBookId(int bookId)
        {
            try
            {
                Book? book = await _repository.GetAsync(filter: x => x.Id == bookId, tracked: false,includeProperties: "Reviews");

                if (book == null)
                {
                    return NotFound("No books exists with this id");
                }

                List<Review> reviews = book.Reviews.ToList();

                List<int> ratings = new List<int>();

                foreach (var item in reviews) 
                {
                    ratings.Add(item.Rating);
                }
                
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = ratings;

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
