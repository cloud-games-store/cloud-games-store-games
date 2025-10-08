using CloudGamesStore.Application.DTOs.GameDtos;
using CloudGamesStore.Application.Interfaces;
using CloudGamesStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CloudGamesStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameSearchController : ControllerBase
{
    private readonly IGameSearchService _service;

    public GameSearchController(IGameSearchService service)
    {
        _service = service;
    }

    [HttpPost("index")]
    public async Task<IActionResult> Index([FromBody] GameDto game)
    {
        await _service.IndexGameAsync(game);
        return Ok("Game indexado com sucesso!");
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var results = await _service.SearchAsync(query);
        return Ok(results);
    }

    [HttpPost("suggest-by-user")]
    public async Task<IActionResult> SuggestByUser([FromBody] IEnumerable<int> userGenreIds)
    {
        var results = await _service.SuggestByUserAsync(userGenreIds);
        return Ok(results);
    }

    [HttpGet("popular-genres")]
    public async Task<IActionResult> PopularGenres()
    {
        var results = await _service.GetPopularGenresAsync();
        return Ok(results);
    }
}