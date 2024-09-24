using System;
using System.Threading.Tasks;
using ApiApplication.Domain.Dtos;
using ApiApplication.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Api.Controllers
{
    [ApiController]
    [Route("[controller]"),]
    public class ShowtimeController : ControllerBase
    {
        private readonly IShowtimeService _showtimeService;
        private readonly IMovieService _movieService;

        public ShowtimeController(IShowtimeService showtimeService, IMovieService movieService)
        {
            _showtimeService = showtimeService;
            _movieService = movieService;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateShowtime([FromBody] ShowtimeCreationRequestDto request)
        {
            try
            {
                var movie = await _movieService.GetMovieById(request.MovieId);
                
                if (movie == null)
                    return NotFound("Movie not found");
                
                var newShowtime = _showtimeService.CreateShowtime(request, movie);
                
                return Ok(newShowtime);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}