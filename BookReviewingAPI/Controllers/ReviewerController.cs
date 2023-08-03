using Azure;
using BookReviewingAPI.Models;
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
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewreRepository _reviewreRepository;
        private APIResponse _response;

        public ReviewerController(IReviewreRepository reviewreRepository)
        {
            _reviewreRepository = reviewreRepository;
            _response = new APIResponse();
        }

        [HttpGet("allReviewers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> GetAllReviewers()
        {
            try
            {
                IEnumerable<Reviewer> reviewers = await _reviewreRepository.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = reviewers;
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

        [HttpGet("reviewer/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetReviewerById(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                Reviewer? reviewer = await _reviewreRepository.GetAsync(filter: x => x.Id == id, tracked: false);
                if (reviewer == null)
                {
                    return NotFound("No reviewers exists with this id");
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = reviewer;

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

        [HttpGet("reviewer/{reviewerId}/reviews")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetReviewsOfReviewer(int reviewerId)
        {
            try
            {
                if (reviewerId == 0)
                {
                    return BadRequest();
                }
                Reviewer? reviewer = await _reviewreRepository.GetAsync(filter: x => x.Id == reviewerId, tracked: false, includeProperties: "Reviews");
                if (reviewer == null)
                {
                    return NotFound();
                }

                string json = JsonConvert.SerializeObject(reviewer, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                var reviewerJson = JsonConvert.DeserializeObject<Reviewer>(json);
                if (reviewerJson == null)
                {
                    return NotFound();
                }

                List<Review> reviewsList = reviewerJson.Reviews.ToList();
                if (reviewsList == null) 
                {
                    return NotFound();
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = reviewsList;

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
