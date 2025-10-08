using CloudGamesStore.Application.DTOs.GameDtos;
using CloudGamesStore.Application.Interfaces;
using CloudGamesStore.Domain.Entities;
using CloudGamesStore.Domain.Interfaces;

namespace CloudGamesStore.Application.Services;

public class GameSearchService : IGameSearchService
{
    private readonly IElasticSearchRepository _repository;

    public GameSearchService(IElasticSearchRepository repository)
    {
        _repository = repository;
    }

    public async Task IndexGameAsync(GameDto gameDto) 
    {
        var game = GameDto.ToGame(gameDto);
        game.Id = gameDto.Id;
        _repository.IndexGameAsync(game);
    } 

    public Task<IEnumerable<Game>> SearchAsync(string query) => _repository.SearchGamesAsync(query);

    public Task<IEnumerable<Game>> SuggestByUserAsync(IEnumerable<int> userGenreIds) =>
        _repository.SuggestGamesByUserAsync(userGenreIds);

    public Task<IEnumerable<(string Genre, long Count)>> GetPopularGenresAsync() =>
        _repository.GetMostPopularGenresAsync();
}
