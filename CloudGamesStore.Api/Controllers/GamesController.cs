using CloudGamesStore.Application.DTOs;
using CloudGamesStore.Application.DTOs.GameDtos;
using CloudGamesStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudGamesStore.Api.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GamesController> _logger;

        public GamesController(IGameService gameService,
            ILogger<GamesController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<ActionResult<List<GameDto>>> All()
        {
            var result = await _gameService.GetAllGames();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<GameDto>>> GetById(int id)
        {
            var result = await _gameService.GetGameById(id);

            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<GameDto>> Create(CreateGameDto createGameDto)
        {
            try
            {
                var result = await _gameService.CreateGame(createGameDto);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating the game {GameName}", createGameDto.Name);
                return StatusCode(500, "An error occurred while creating an game");
            }
        }

        [HttpPost("Update")]
        public async Task<ActionResult<GameDto>> Update(UpdateGameDto gameDto)
        {
            try
            {
                var result = await _gameService.UpdateGame(gameDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating the game {GameName}", gameDto.Name);
                return StatusCode(500, "An error occurred while updating an game");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _gameService.DeleteGame(id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting the game {GameId}", id);
                return StatusCode(500, "An error occurred while creating an game");
            }
        }
    }
}
