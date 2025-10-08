using CloudGamesStore.Domain.Entities;
using CloudGamesStore.Domain.Interfaces;
using OpenSearch.Client;

namespace CloudGamesStore.Infrastructure.Elasticsearch;

public class ElasticSearchRepository : IElasticSearchRepository
{
    private readonly IOpenSearchClient _client;
    private readonly string _index = "cloudgames-games";

    public ElasticSearchRepository(IOpenSearchClient client)
    {
        _client = client;
    }

    public async Task IndexGameAsync(Game game)
    {
        await _client.IndexDocumentAsync(game);
    }

    public async Task<IEnumerable<Game>> SearchGamesAsync(string query, int take = 20)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<Game>();

        var response = await _client.SearchAsync<Game>(s => s
            .Index(_index)
            .Size(take)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(query)
                    .Fields(f => f
                        .Field(p => p.Name)
                        .Field(p => p.Description)
                    )
                    .Type(TextQueryType.BestFields)
                    .Fuzziness(Fuzziness.Auto)       
                    .Operator(Operator.Or)           
                )
            )
        );

        if (!response.IsValid)
        {
            Console.WriteLine($"[OpenSearch] Erro na busca: {response.OriginalException?.Message}");
            return Enumerable.Empty<Game>();
        }

        return response.Documents;
    }

    public async Task<IEnumerable<Game>> SuggestGamesByUserAsync(IEnumerable<int> userGenreIds, int take = 10)
    {
        var response = await _client.SearchAsync<Game>(s => s
            .Index(_index)
            .Query(q => q
                .Bool(b => b
                    .Must(m => m.Terms(t => t.Field(f => f.GenreId).Terms(userGenreIds)))
                    .Filter(f => f.Term(t => t.Field(f => f.IsActive).Value(true)))
                )
            )
            .Size(take)
        );

        return response.Documents;
    }

    public async Task<IEnumerable<(string Genre, long Count)>> GetMostPopularGenresAsync()
    {
        var response = await _client.SearchAsync<Game>(s => s
            .Size(0)
            .Aggregations(a => a
                .Terms("popular_genres", t => t
                    .Field(f => f.Genre.Suffix("keyword"))
                )
            )
        );

        var agg = response.Aggregations.Terms("popular_genres");
        return agg.Buckets.Select(b => (b.Key as string, b.DocCount ?? 0));
    }
}