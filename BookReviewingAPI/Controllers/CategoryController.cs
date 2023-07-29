using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace BookReviewingAPI.Controllers
{
    [Route("api/")]
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
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CategoryById(int categoryId) 
        {
            try 
            {
                if (categoryId == 0)
                {
                    return BadRequest();
                }
                Category category = await _categoryRepository.GetAsync(filter: x => x.Id == categoryId, tracked: false);
                if (category == null)
                {
                    return NotFound("No category found with this id");
                }
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = category;
                return _apiResponse;
            }catch(Exception e) 
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessage = new List<string>() { e.ToString() };
                return _apiResponse;
            }
        }
        [HttpGet("categories/bookId/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CategoriesByBookId(int bookId) 
        {

        }
    }
}
