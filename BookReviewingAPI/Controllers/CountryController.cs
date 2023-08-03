﻿using Azure;
using BookReviewingAPI.Models;
using BookReviewingAPI.Repository.IRepository;
using BookReviewingAPI.Repository.IRepositoryImpl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Net;
using System.Text.Json.Nodes;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace BookReviewingAPI.Controllers
{
    [Route("api/[Controller]/")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IAuthorRepository _authorRepository;
        private APIResponse _response;

        public CountryController(ICountryRepository repository,IAuthorRepository authorRepository)
        {
            _countryRepository = repository;
            _response = new APIResponse();
            _authorRepository = authorRepository;
        }
        [HttpGet("allCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllCountries()
        {
            try
            {
                IEnumerable<Country> countries = await _countryRepository.GetAllAsync();
                if (countries == null) 
                {
                    return NotFound("No countries exists with this id");
                }
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetCountryById(int countryId)
        {
            try
            {
                if (countryId == 0)
                {
                    return BadRequest();
                }
                Country? country = await _countryRepository.GetAsync(filter: x => x.Id == countryId, tracked: false);
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
      
        [HttpGet("{authorId}/country")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //TODO:FIX THIS BUG
        public async Task<ActionResult<APIResponse>> GetCountryByAuthorId(int authorId)
         {
             try 
             {
                 if (authorId == 0)
                 {
                     return BadRequest();
                 }
                 Author? author = await _authorRepository.GetAsync(filter: x => x.Id == authorId, includeProperties: "Country");
                 if (author == null)
                 {
                     return NotFound("No authors exists with this id");
                 }
                 string json = JsonConvert.SerializeObject(author, Formatting.Indented, new JsonSerializerSettings
                 {
                     ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                 });
                 Country? country = JsonConvert.DeserializeObject<Country>(json);
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

        [HttpGet("{countryId}/authors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //TODO:FIX THIS BUG
        public async Task<ActionResult<APIResponse>> GetAuthorsByCountryId(int countryId)
        {
            try
            {
                if (countryId == 0)
                {
                    return BadRequest();
                }
                Country? country = await _countryRepository.GetAsync(filter: x => x.Id == countryId, includeProperties: "Authors");
                if (country == null)
                {
                    return NotFound("No countries exists with this id");
                }

                string json = JsonConvert.SerializeObject(country, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                List<Author>? authors = JsonConvert.DeserializeObject<List<Author>>(json);


                if (authors == null)
                {
                    return NotFound("No authors exists with this id");
                }
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
    }
}