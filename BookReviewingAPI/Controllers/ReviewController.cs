using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.DTOS;
using BookReviewingAPI.Repository.IRepository;
using BookReviewingAPI.Repository.IRepositoryImpl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace BookReviewingAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookRepository _bookRepository;
        private APIResponse _response;
        private IMapper _mapper;
        public ReviewController(IReviewRepository reviewRepository, IBookRepository bookRepository,IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _response = new APIResponse();
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet("allReviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllReviews()
        {
            try
            {
                IEnumerable<Review> reviews = await _reviewRepository.GetAllAsync();
                if (reviews == null) 
                {
                    return NotFound();
                }
                List<ReviewDTO> reviewDTOs = _mapper.Map<List<ReviewDTO>>(reviews);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = reviewDTOs;
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

        [HttpGet("review/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetReviewById(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                Review? review = await _reviewRepository.GetAsync(filter: x => x.Id == id, tracked: false);
                if (review == null)
                {
                    return NotFound("No reviews exists with this id");
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = review;

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

        [HttpGet("reviews/bookId/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetReviewsByBookId(int bookId)
        {
            try 
            {
                if (bookId == 0)
                {
                    return BadRequest();
                }
                Book? book = await _bookRepository.GetAsync(filter: x => x.Id == bookId, tracked: false, includeProperties: "Reviews");
                if (book == null)
                {
                    return NotFound();
                }
                string json = JsonConvert.SerializeObject(book, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                var bookJson = JsonConvert.DeserializeObject<Book>(json);
                if (bookJson == null)
                {
                    return NotFound();
                }

                List<Review> reviews = bookJson.Reviews.ToList();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = reviews;

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

        [HttpGet("book/reviewId/{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBookByReviewId(int reviewId) 
        {
            try
            {
                if (reviewId == 0)
                {
                    return BadRequest();
                }
                Review? review = await _reviewRepository.GetAsync(filter: x => x.Id == reviewId, tracked: false ,includeProperties:"Book");
                if (review == null)
                {
                    return NotFound("No reviews exists with this id");
                }
                string json = JsonConvert.SerializeObject(review, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                var reviewJson = JsonConvert.DeserializeObject<Review>(json);
                if (reviewJson == null)
                {
                    return NotFound();
                }
                Book? book = reviewJson.Book;
                if (book == null)
                {
                    return NotFound();
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
