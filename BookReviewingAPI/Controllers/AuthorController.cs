﻿using Azure;
using BookReviewingAPI.Models;
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
        private APIResponse _response;
        public AuthorController(IAuthorRepository repository, IBookRepository bookRepository)
        {
            _authorRepository = repository;
            _response = new APIResponse();
            _bookRepository = bookRepository;
        }
        [HttpGet("allAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> GetAllAuthors()
        {
            try
            {
                IEnumerable<Author> authors = await _authorRepository.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = authors;
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
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = author;

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

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = authors;

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

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = books;

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
