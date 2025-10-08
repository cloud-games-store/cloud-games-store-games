using CloudGamesStore.Application.DTOs.GameDtos;
using CloudGamesStore.Domain.Entities;

namespace CloudGamesStore.Application.Interfaces;

public interface IGameSearchService
{
    public Task IndexGameAsync(GameDto game);
    public Task<IEnumerable<Game>> SearchAsync(string query);
    public Task<IEnumerable<Game>> SuggestByUserAsync(IEnumerable<int> userGenreIds);
    public Task<IEnumerable<(string Genre, long Count)>> GetPopularGenresAsync();
}
