using AutoMapper;
using Azure;
using BookReviewingAPI.Models;
using BookReviewingAPI.Models.DTOS;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookReviewingAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ICountryRepository _countryRepository;
        private APIResponse _response;
        private IMapper _mapper;
        public AuthorController(IAuthorRepository repository, IBookRepository bookRepository, IMapper mapper, ICountryRepository countryRepository)
        {
            _authorRepository = repository;
            _response = new APIResponse();
            _bookRepository = bookRepository;
            _countryRepository = countryRepository;
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
                IEnumerable<Author> authors = await _authorRepository.GetAllAsync();

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
                Author? author = await _authorRepository.GetAsync(filter: x => x.Id == id, tracked: false);
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
                Book? book = await _bookRepository.GetAsync(filter: x => x.Id == bookId);

                if (book == null)
                {
                    return NotFound("No books found with this id");
                }
                List<Author> authors = _authorRepository.GetAuthorByBookId(bookId);
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
                Author? author = await _authorRepository.GetAsync(filter: x => x.Id == authorId);

                if (author == null)
                {
                    return NotFound("No authors found with this id");
                }
                List<Book> books = await _bookRepository.GetBooksByAuthorId(authorId);
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

                var country = await _countryRepository.GetAsync(filter:x => x.Id == authorToCreate.Country.Id);

                if (country == null)
                {
                    ModelState.AddModelError("", "Country doesn't exist!");
                    return StatusCode(404, ModelState);
                }

                authorToCreate.Country = country;

                await _authorRepository.CreateAsync(authorToCreate);
                await _authorRepository.SaveChanges();

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
    }
}
