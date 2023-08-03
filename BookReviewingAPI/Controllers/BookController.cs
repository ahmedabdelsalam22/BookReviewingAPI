using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.DTOS;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.InteropServices;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookReviewingAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _repository;
        private APIResponse _response;
        private IMapper _mapper;
        public BookController(IBookRepository repository, IMapper mapper)
        {
            _repository = repository;
            _response = new APIResponse();
            _mapper = mapper;
        }

        [HttpGet("allBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllBooks()
        {
            try
            {
                IEnumerable<Book> books = await _repository.GetAllAsync();
                if (books == null) 
                {
                    return NotFound();
                }
                List<BookDTO> bookDTOs = _mapper.Map<List<BookDTO>>(books);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = bookDTOs;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                BookDTO bookDTO = _mapper.Map<BookDTO>(book);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = bookDTO;

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
                BookDTO bookDTO = _mapper.Map<BookDTO>(book);

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

                List<ReviewDTO> reviewDTOs = _mapper.Map<List<ReviewDTO>>(reviews);

                List<int> ratings = new List<int>();

                foreach (var item in reviewDTOs) 
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
