using Azure;
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
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _repository;
        private APIResponse _response;

        public CountryController(ICountryRepository repository)
        {
            _repository = repository;
            _response = new APIResponse();
        }
        [HttpGet("allCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<APIResponse> GetAllCountries()
        {
            try
            {
                IEnumerable<Country> countries = await _repository.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = countries;
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
        [HttpGet("country/{countryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCountryById(int countryId)
        {
            try
            {
                if (countryId == 0)
                {
                    return BadRequest();
                }
                Country? country = await _repository.GetAsync(filter: x => x.Id == countryId, tracked: false);
                if (country == null)
                {
                    return NotFound("No countries exists with this id");
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = country;

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
