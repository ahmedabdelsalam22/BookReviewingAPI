﻿using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookReviewingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepository _categoryRepository;
        private readonly APIResponse _apiResponse;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _apiResponse = new APIResponse();
        }

        [HttpGet("allCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAllCategories()
        {
            try
            {
                List<Category> categories = await _categoryRepository.GetAllAsync();
                if (categories == null)
                {
                    return NotFound();
                }
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = categories;
                return _apiResponse;
            }
            catch (Exception e) 
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessage = new List<string>() { e.ToString() };
                return _apiResponse;
            }
        }
    }
}
