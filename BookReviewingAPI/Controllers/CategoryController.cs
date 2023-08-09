using AutoMapper;
using Azure;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.DTOS;
using BookReviewingAPI.Repository.IRepository;
using BookReviewingAPI.Repository.IRepositoryImpl;
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
        private IBookRepository _bookRepository;
        private readonly APIResponse _apiResponse;
        private IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IBookRepository bookRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _apiResponse = new APIResponse();
            _bookRepository = bookRepository;
            _mapper = mapper;
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
                List<CategoryDTO> categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = categoryDTOs;
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
        public async Task<ActionResult<APIResponse>> GetCategoryById(int categoryId) 
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
                CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = categoryDTO;
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
        public async Task<ActionResult<APIResponse>> GetCategoriesByBookId(int bookId) 
        {
            try 
            {
                Book? book = await _bookRepository.GetAsync(filter: x => x.Id == bookId, tracked: false);
                if (book == null)
                {
                    return NotFound();
                }
                List<Category> categories = await _categoryRepository.GetCategoriesByBookId(bookId);
                if (categories == null)
                {
                    return NotFound();
                }
                List<CategoryDTO> categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);


                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = categoryDTOs;
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
        [HttpGet("books/categoryId/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetBooksByCategoryId(int categoryId)
        {
            try
            {
                Category? category = await _categoryRepository.GetAsync(filter: x => x.Id == categoryId, tracked: false);
                if (category == null)
                {
                    return NotFound();
                }
                List<Book> books = await _bookRepository.GetBooksByCategoryId(categoryId);
                if (books == null)
                {
                    return NotFound();
                }

                List<BookDTO> bookDTOs = _mapper.Map<List<BookDTO>>(books);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = bookDTOs;
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

        [HttpPost("category/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null)
                {
                    return BadRequest(ModelState);
                }
                var category = await _categoryRepository.GetAsync(filter: x => x.Name.ToLower() == categoryDTO.Name.ToLower());
                if (category != null)
                {
                    return BadRequest("this category already exists");
                }

                Category categoryToDB = _mapper.Map<Category>(categoryDTO);

                await _categoryRepository.CreateAsync(categoryToDB);
                await _categoryRepository.SaveChanges();

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = categoryToDB;
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

        [HttpPut("category/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateCategory([FromBody] CategoryDTO categoryDTO, int categoryId)
        {
            try
            {
                if (categoryDTO == null)
                {
                    return BadRequest(ModelState);
                }
                if (categoryId != categoryDTO.Id)
                {
                    return BadRequest(ModelState);
                }

                Category category = await _categoryRepository.GetAsync(filter: x => x.Id == categoryId, tracked: false);
                if (category == null)
                {
                    return NotFound(ModelState);
                }
                if (category.Name == categoryDTO.Name)
                {
                    return BadRequest("sorry! its the same name .. please update it");
                }

                Category categoryToDB = _mapper.Map<Category>(categoryDTO);

                _categoryRepository.Update(categoryToDB);
                await _categoryRepository.SaveChanges();

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = categoryToDB;
                return _apiResponse;
            }
            catch (Exception e)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotModified;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string> { e.ToString() };
                return _apiResponse;
            }

        }

        [HttpDelete("category/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int categoryId)
        {
            try
            {
                Category category = await _categoryRepository.GetAsync(filter: x => x.Id == categoryId);
                if (category == null)
                {
                    return NotFound("category does't exists");
                }
                List<Book> books = await _bookRepository.GetBooksByCategoryId(categoryId);
                if (books.Count() > 0)
                {
                    ModelState.AddModelError("", $"Category {category.Name} cannot be deleted because it is used by at least one book");
                }
                _categoryRepository.Delete(category);
                await _categoryRepository.SaveChanges();

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return _apiResponse;
            }
            catch (Exception e) 
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string> { e.ToString() };
                return _apiResponse;
            }

        }
    }
}
