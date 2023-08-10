using AutoMapper;
using Azure;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.DTOS;
using BookReviewingAPI.Repository.IRepository;
using BookReviewingAPI.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace BookReviewingAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private APIResponse _response;
        private IMapper _mapper;
        public AuthorController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _response = new APIResponse();
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet("allAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> GetAllAuthors()
        {
            try
            {
                IEnumerable<Author> authors = await _unitOfWork.authorRepository.GetAllAsync();

                List<AuthorDTO> authorsDTO = _mapper.Map<List<AuthorDTO>>(authors);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = authorsDTO;
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
        [HttpGet("author/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAuthorById(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                Author? author = await _unitOfWork.authorRepository.GetAsync(filter: x => x.Id == id, tracked: false);
                if (author == null)
                {
                    return NotFound("No authors exists with this id");
                }

                AuthorDTO authorDTO = _mapper.Map<AuthorDTO>(author);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = authorDTO;

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

        [HttpGet("{bookId}/authors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAuthorByBookId(int bookId) 
        {
            try
            {
                Book? book = await _unitOfWork.bookRepository.GetAsync(filter: x => x.Id == bookId);

                if (book == null)
                {
                    return NotFound("No books found with this id");
                }
                List<Author> authors = _unitOfWork.authorRepository.GetAuthorByBookId(bookId);
                if (authors == null) 
                {
                    return NotFound();
                }
                List<AuthorDTO> authorDTO = _mapper.Map<List<AuthorDTO>>(authors);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = authorDTO;

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

        [HttpGet("{authorId}/books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBooksByAuthorId(int authorId) 
        {
            try
            {
                Author? author = await _unitOfWork.authorRepository.GetAsync(filter: x => x.Id == authorId);

                if (author == null)
                {
                    return NotFound("No authors found with this id");
                }
                List<Book> books = await _unitOfWork.bookRepository.GetBooksByAuthorId(authorId);
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

        [HttpPost("authors/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateAuthor([FromBody] Author authorToCreate)
        {
            try 
            {
                if (authorToCreate == null)
                {
                    return BadRequest(ModelState);
                }

                var country = await _unitOfWork.countryRepository.GetAsync(filter:x => x.Id == authorToCreate.Country.Id);

                if (country == null)
                {
                    ModelState.AddModelError("", "Country doesn't exist!");
                    return StatusCode(404, ModelState);
                }

                authorToCreate.Country = country;

                await _unitOfWork.authorRepository.CreateAsync(authorToCreate);
                await _unitOfWork.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = authorToCreate;
                return _response;
            }
            catch (Exception e) 
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage = new List<string>() {e.ToString() };
                return _response;
            }
        }
        [HttpPut("authors/{authorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateAuthor(int authorId, [FromBody] Author authorToUpdate)
        {
            try
            {
                if (authorToUpdate == null)
                {
                    return BadRequest(ModelState);
                }
                if (authorId != authorToUpdate.Id)
                {
                    return BadRequest(ModelState);
                }
                Author author = await _unitOfWork.authorRepository.GetAsync(filter: x => x.Id == authorId,tracked:false);
                if (author == null)
                {
                    return NotFound("No author exists with this id");
                }
                Country country = await _unitOfWork.countryRepository.GetAsync(filter: x => x.Id == authorToUpdate.Country.Id);
                if (country == null)
                {
                    return NotFound("No country exists with this id");
                }
                authorToUpdate.Country = country;

                _unitOfWork.authorRepository.UpdateAsync(authorToUpdate);
                await _unitOfWork.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = authorToUpdate;
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

        [HttpDelete("author/{authorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> DeleteAuthor(int authorId)
        {
            try
            {
                Author author = await _unitOfWork.authorRepository.GetAsync(filter: x => x.Id == authorId);
                if (author == null)
                {
                    return NotFound("no authors found with this id");
                }
                // related entities
                List<Book> books = await _unitOfWork.bookRepository.GetBooksByAuthorId(authorId);
                if (books.Count() > 0)
                {
                    ModelState.AddModelError(
                        "CustomError",
                        $"Author {author.FirstName} {author.LastName} cannot be deleted because it is associated with at least one book"
                        );
                    return StatusCode(409, ModelState);
                }

                _unitOfWork.authorRepository.Delete(author);
                await _unitOfWork.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Result = "author deleted successfuly";
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
