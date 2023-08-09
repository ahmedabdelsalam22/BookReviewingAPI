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
        private readonly IReviewerRepository _reviewerRepository;
        private APIResponse _response;
        private IMapper _mapper;
        public ReviewController(IReviewRepository reviewRepository, IBookRepository bookRepository,IMapper mapper, IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _response = new APIResponse();
            _bookRepository = bookRepository;
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
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

                ReviewDTO reviewDTO = _mapper.Map<ReviewDTO>(review);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = reviewDTO;

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
                //string json = JsonConvert.SerializeObject(book, Formatting.Indented, new JsonSerializerSettings
                //{
                //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //});

                //var bookJson = JsonConvert.DeserializeObject<Book>(json);
                //if (bookJson == null)
                //{
                //    return NotFound();
                //}

                List<Review> reviews = book.Reviews.ToList();

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

        [HttpPost("review/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateReview([FromBody] Review reviewToCreate)
        {
            try 
            {
                if (reviewToCreate == null)
                {
                    return BadRequest(ModelState);
                }
                // related entities 
                Book book = await _bookRepository.GetAsync(filter: x => x.Id == reviewToCreate.Book.Id);
                Reviewer reviewer = await _reviewerRepository.GetAsync(filter: x => x.Id == reviewToCreate.Reviewer.Id);
                // check is related entities(Parent tables) is found or not..
                if (book == null)
                {
                    return BadRequest("book is't exists");
                }
                if (reviewer == null)
                {
                    return BadRequest("book is't exists");
                }

                reviewToCreate.Book = book;
                reviewToCreate.Reviewer = reviewer;


                await _reviewRepository.CreateAsync(reviewToCreate);
                await _reviewRepository.SaveChanges();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = reviewToCreate;
                return _response;
            }
            catch(Exception e)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = new List<string>() { e.ToString() };
                _response.IsSuccess = false;

                return _response;
            }

        }

        [HttpPut("review/{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateReview(int reviewId, [FromBody] Review reviewToUpdate)
        {
            try
            {
                if (reviewToUpdate == null)
                {
                    return BadRequest(ModelState);
                }
                if (reviewId != reviewToUpdate.Id)
                {
                    return BadRequest(ModelState);
                }
                Review review = await _reviewRepository.GetAsync(filter: x => x.Id == reviewId, tracked: false);
                if (review == null)
                {
                    return NotFound("No review exists with this id");
                }
                Book book = await _bookRepository.GetAsync(filter: x => x.Id == reviewToUpdate.Book.Id);
                if (book == null)
                {
                    return NotFound("No book exists with this id");
                }
                Reviewer reviewer = await _reviewerRepository.GetAsync(filter: x => x.Id == reviewToUpdate.Reviewer.Id);
                if (book == null)
                {
                    return NotFound("No reviewer exists with this id");
                }
                reviewToUpdate.Book = book;
                reviewToUpdate.Reviewer = reviewer;

                _reviewRepository.Update(reviewToUpdate);
                await _reviewRepository.SaveChanges();

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = reviewToUpdate;
                return _response;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = new List<string>() { e.ToString() };
                return _response;
            }

        }

        [HttpDelete("review/{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteReview(int reviewId)
        {
            try
            {
                Review review = await _reviewRepository.GetAsync(filter: x => x.Id == reviewId);
                if (review == null)
                {
                    return NotFound("review does't exists");
                }
                _reviewRepository.Delete(review);
                await _reviewRepository.SaveChanges();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "review removed successfully";
                return _response;
            }
            catch (Exception e) 
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = new List<string>() { e.ToString() };
                return _response;
            }

        }
    }
}
