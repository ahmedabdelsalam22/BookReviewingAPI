using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.DTOS;
using BookReviewingAPI.Repository.IRepository;
using BookReviewingAPI.Repository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookReviewingAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private APIResponse _response;
        private IMapper _mapper;
        public BookController(IMapper mapper , IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                IEnumerable<Book> books = await _unitOfWork.bookRepository.GetAllAsync();
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
                Book? book = await _unitOfWork.bookRepository.GetAsync(filter: x => x.Id == bookId, tracked: false);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetBookByISBN(string isbn)
        {
            try
            {
                Book? book = await _unitOfWork.bookRepository.GetAsync(filter: x => x.Isbn == isbn, tracked: false);
                if (book == null)
                {
                    return NotFound("No books exists with this isbn");
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

        [HttpGet("book/{bookId}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRatingByBookId(int bookId)
        {
            try
            {
                Book? book = await _unitOfWork.bookRepository.GetAsync(filter: x => x.Id == bookId, tracked: false,includeProperties: "Reviews");

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

        [HttpPost("book/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateBook(BookCreateDTO bookCreateDTO)
        {
            try
            {
                if (bookCreateDTO == null)
                {
                    return BadRequest(ModelState);
                }
                var book = await _unitOfWork.bookRepository.GetAsync(filter: x => x.Title.ToLower() == bookCreateDTO.Title.ToLower());
                if (book != null)
                {
                    return BadRequest("this book already exists");
                }

                Book bookToDB = _mapper.Map<Book>(bookCreateDTO);
                await _unitOfWork.bookRepository.CreateAsync(bookToDB);
                await _unitOfWork.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = bookToDB;
                return _response;
            }
            catch (Exception e)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = new List<string>() { e.ToString() };
                _response.IsSuccess = false;

                return _response;
            }

        }

        [HttpPut("book/bookId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        public async Task<ActionResult<APIResponse>> UpdateBook([FromBody] BookDTO bookDTO,int bookId)
        {
            try 
            {
                if (bookDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (bookId != bookDTO.Id)
                {
                    return BadRequest(ModelState);
                }
                Book book = await _unitOfWork.bookRepository.GetAsync(filter: x => x.Id == bookId,tracked:false);
                if (book == null)
                {
                    return NotFound("no book exists with this id");
                }

                Book bookToDB = _mapper.Map<Book>(bookDTO);

                _unitOfWork.bookRepository.Update(bookToDB);
                await _unitOfWork.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = bookToDB;
                return _response;

            }catch(Exception e) 
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = new List<string>() { e.ToString() };
                _response.IsSuccess = false;

                return _response;
            }
        }

        [HttpDelete("book/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> DeleteBook(int bookId)
        {
            try
            {
                Book? book = await _unitOfWork.bookRepository.GetAsync(filter: x => x.Id == bookId, includeProperties: "Reviews");
                if (book == null)
                {
                    return NotFound("No book found with this id");
                }
                // get all reviews for this book to deleted with book
                List<Review> reviews = book.Reviews.ToList();

                _unitOfWork.reviewRepository.DeleteReviews(reviews);
                await _unitOfWork.SaveChangesAsync();

                _unitOfWork.bookRepository.Delete(book);
                await _unitOfWork.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return _response;

            }
            catch (Exception e) 
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = new List<string>() { e.ToString() };
                _response.IsSuccess = false;

                return _response;
            }

        }

    }

}
