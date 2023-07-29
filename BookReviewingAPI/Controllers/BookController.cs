using AutoMapper;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookRepository _repository;

        public BookController(IBookRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("allBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            IEnumerable<Book> books = await _repository.GetAllAsync();
            return Ok(books);
        }


    }
}
