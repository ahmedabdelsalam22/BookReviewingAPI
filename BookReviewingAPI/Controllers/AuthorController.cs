using Azure;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookReviewingAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private APIResponse _response; 
        public AuthorController(IAuthorRepository repository, IBookRepository bookRepository)
        {
            _authorRepository = repository;
            _response = new APIResponse();
            _bookRepository = bookRepository;
        }
        [HttpGet("allAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> GetAllAuthors()
        {
            try
            {
                IEnumerable<Author> authors = await _authorRepository.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = authors;
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
        [HttpGet("author/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAuthorById(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                Author? author = await _authorRepository.GetAsync(filter: x => x.Id == id, tracked: false);
                if (author == null)
                {
                    return NotFound("No authors exists with this id");
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = author;

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

        [HttpGet("authors/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAuthorByBookId(int bookId) 
        {
            try
            {
                Book? book = await _bookRepository.GetAsync(filter: x => x.Id == bookId, includeProperties: "BookAuthors");

                if (book == null)
                {
                    return NotFound("No books found with this id");
                }

                List<BookAuthor> bookAuthors = book.BookAuthors.ToList();

                List<Author> bookAuthorsList = new List<Author>();

                foreach (var item in bookAuthors)
                {
                    bookAuthorsList.Add(item.Author);
                }
                if (bookAuthorsList == null)
                {
                    return NotFound("No authors found with this bookId");
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = bookAuthorsList;

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
