using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using BookReviewingAPI.Repository.IRepositoryImpl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookReviewingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private APIResponse _response;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
            _response = new APIResponse();
        }

        [HttpGet("allReviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> GetAllReviews()
        {
            try
            {
                IEnumerable<Review> reviews = await _reviewRepository.GetAllAsync();
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

    }
}
