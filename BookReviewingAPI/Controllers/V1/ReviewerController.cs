﻿using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.DTOS;
using BookReviewingAPI.Repository.IRepository;
using BookReviewingAPI.Repository.IRepositoryImpl;
using BookReviewingAPI.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Net;

namespace BookReviewingAPI.Controllers.V1
{
    [Route("api/v{version:apiVersion}/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ReviewerController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private APIResponse _response;
        private IMapper _mapper;

        public ReviewerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _response = new APIResponse();
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("allReviewers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllReviewers()
        {
            try
            {
                IEnumerable<Reviewer> reviewers = await _unitOfWork.reviewerRepository.GetAllAsync();
                if (reviewers == null)
                {
                    return NotFound();
                }
                List<ReviewerDTO> reviewDTOs = _mapper.Map<List<ReviewerDTO>>(reviewers);

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

        [Authorize]
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
                Reviewer? reviewer = await _unitOfWork.reviewerRepository.GetAsync(filter: x => x.Id == id, tracked: false);
                if (reviewer == null)
                {
                    return NotFound("No reviewers exists with this id");
                }
                ReviewerDTO reviewDTOs = _mapper.Map<ReviewerDTO>(reviewer);

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

        [Authorize]
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
                Reviewer? reviewer = await _unitOfWork.reviewerRepository.GetAsync(filter: x => x.Id == reviewerId, tracked: false, includeProperties: "Reviews");
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

        [Authorize]
        [HttpGet("reviewers/{reviewId}/reviewer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetReviewerByReviewId(int reviewId)
        {
            try
            {
                if (reviewId == 0)
                {
                    return BadRequest();
                }
                Review? review = await _unitOfWork.reviewRepository.GetAsync(filter: x => x.Id == reviewId, tracked: false, includeProperties: "Reviewer");
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
                Reviewer reviewer = reviewJson.Reviewer;
                if (reviewer == null)
                {
                    return NotFound();
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

        [Authorize(Roles = "admin")]
        [HttpPost("reviewer/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateReviewer(Reviewer reviewer)
        {
            try
            {
                if (reviewer == null)
                {
                    return BadRequest(ModelState);
                }

                await _unitOfWork.reviewerRepository.CreateAsync(reviewer);
                await _unitOfWork.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = reviewer;
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

        [Authorize(Roles = "admin")]
        [HttpPut("reviewer/reviewerId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        public async Task<ActionResult<APIResponse>> UpdateReviewer([FromBody] ReviewerDTO reviewerDTO, int reviewerId)
        {
            try
            {
                if (reviewerDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (reviewerId != reviewerDTO.Id)
                {
                    return BadRequest(ModelState);
                }
                Reviewer reviewer = await _unitOfWork.reviewerRepository.GetAsync(filter: x => x.Id == reviewerId, tracked: false);
                if (reviewer == null)
                {
                    return NotFound("no reviewer exists with this id");
                }

                Reviewer reviewerToDB = _mapper.Map<Reviewer>(reviewerDTO);

                _unitOfWork.reviewerRepository.Update(reviewerToDB);
                await _unitOfWork.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = reviewerToDB;
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

        [Authorize(Roles = "admin")]
        [HttpDelete("reviewer/{reviewerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteReviewer(int reviewerId)
        {
            try
            {
                Reviewer reviewer = await _unitOfWork.reviewerRepository.GetAsync(filter: x => x.Id == reviewerId, includeProperties: "Reviews");
                if (reviewer == null)
                {
                    return NotFound("reviewer does't exists");
                }
                List<Review>? reviews = reviewer.Reviews.ToList();
                _unitOfWork.reviewRepository.DeleteReviews(reviews);
                await _unitOfWork.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = "reviewer removed successfully";
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
