using CloudGamesStore.Domain.Entities;

namespace CloudGamesStore.Domain.Interfaces;

public interface IElasticSearchRepository
{
    public Task<IEnumerable<(string Genre, long Count)>> GetMostPopularGenresAsync();
    public Task<IEnumerable<Game>> SuggestGamesByUserAsync(IEnumerable<int> userGenreIds, int take = 10);
    public Task<IEnumerable<Game>> SearchGamesAsync(string query, int take = 20);
    public Task IndexGameAsync(Game game);
}
