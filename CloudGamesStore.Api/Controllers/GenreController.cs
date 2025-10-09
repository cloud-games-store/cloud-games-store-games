using CloudGamesStore.Application.DTOs.GenreDtos;
using CloudGamesStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudGamesStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly ILogger<GenreController> _logger;

        public GenreController(IGenreService genreService,
            ILogger<GenreController> logger)
        {
            _genreService = genreService;
            _logger = logger;
        }

        [HttpGet()]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<GenreDto>>> All()
        {
            var result = await _genreService.GetAllGenres();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<GenreDto>>> GetById(int id)
        {
            var result = await _genreService.GetGenreById(id);

            return Ok(result);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GenreDto>> Create(CreateGenreDto createGenreDto)
        {
            try
            {
                var result = await _genreService.CreateGenre(createGenreDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating the genre {GenreName}", createGenreDto.Name);
                return StatusCode(500, "An error occurred while creating an genre");
            }
        }

        [HttpPost("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GenreDto>> Update(UpdateGenreDto genreDto)
        {
            try
            {
                var result = await _genreService.UpdateGenre(genreDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating the genre {GenreName}", genreDto.Name);
                return StatusCode(500, "An error occurred while updating an genre");
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _genreService.DeleteGenre(id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting the genre {GenreId}", id);
                return StatusCode(500, "An error occurred while deleting an genre");
            }
        }
    }
}
