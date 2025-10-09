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

        // decide se usamos fuzziness: evita fuzz para termos curtos (ruído)
        var useFuzziness = query.Length >= 3;

        // Epoch - corresponde ao formato epoch_millis que o Elasticsearch aceita para datas
        // converter DateTime.UtcNow para epoch milliseconds (double)
        var epochNow = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;

        var response = await _client.SearchAsync<Game>(s => s
            .Index(_index)
            .Query(q => q
                // function score para combinar relevância textual + sinais simples (recência/preço)
                .FunctionScore(fs => fs
                    // query base: multi-match (nome + descrição), com boost no campo Name
                    .Query(qq => qq.MultiMatch(m =>
                    {
                        m.Query(query);
                        m.Type(TextQueryType.BestFields);
                        m.Fields(f => f
                            .Field(p => p.Name, 3) // Name^3
                            .Field(p => p.Description) // Description^1
                        );
                        m.TieBreaker(0.2); // combina scores entre campos
                        m.Operator(Operator.Or);

                        if (useFuzziness)
                            m.Fuzziness(Fuzziness.Auto);

                        return m;
                    }))
                    // funções que alteram levemente o score (não substituem relevância textual)
                    .Functions(func => func
                        // boost para jogos mais recentes (gauss/decay)
                        .Gauss(g => g
                            .Field(f => f.CreatedAt)
                            .Origin(epochNow) // ponto de referência ("agora" por ser data)
                            .Offset(7) // não penaliza a primeira semana
                            .Scale(180) // meia-vida de 180 dias
                            .Decay(0.5)
                        )
                        // Leve preferência para jogos mais baratos
                        .FieldValueFactor(ff => ff
                            .Field(f => f.Price)
                            .Modifier(FieldValueFactorModifier.Reciprocal) // Reciprocal: - menor preço => maior score
                            .Factor(1) // fator de escala
                            .Missing(1) // evita quebrar o cálculo do score na falta do campo 
                        )
                    )
                    // combinação: multiplica o score textual pelo resultado das funções
                    .ScoreMode(FunctionScoreMode.Multiply) // Combina score da função com score da query
                    .BoostMode(FunctionBoostMode.Multiply) // Combina resultado final com boost da query
                )
            )
            .Size(take)
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
        var genres = userGenreIds?.ToList() ?? new List<int>();
        var hasGenres = genres.Count != 0;

        // Epoch - corresponde ao formato epoch_millis que o Elasticsearch aceita para datas
        // converter DateTime.UtcNow para epoch milliseconds (double)
        var epochNow = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;

        var response = await _client.SearchAsync<Game>(s => s
            .Index(_index)
            .Query(q =>
                q.FunctionScore(fs => fs
                    .Query(q2 => q2.Bool(b => b
                        .Filter(f => f.Term(t => t.Field(f => f.IsActive).Value(true))) // apenas ativos
                        .Filter(f => hasGenres
                            ? f.Terms(t => t.Field(ff => ff.GenreId).Terms(genres))
                            : f.MatchAll()) // fallback: Se não houver gêneros, retorna jogos mais novos e ativos
                    ))
                    .Functions(func => func
                        // Boost para jogos mais recentes (decay/gauss)
                        .Gauss(g => g
                            .Field(f => f.CreatedAt)
                            .Origin(epochNow) // ponto de referência ("agora" por ser data)
                            .Offset(7) // não penaliza a primeira semana
                            .Scale(180) // meia-vida de 180 dias
                            .Decay(0.5)
                        )
                        // Leve preferência para jogos mais baratos
                        .FieldValueFactor(fvf => fvf
                            .Field(f => f.Price)
                            .Factor(1) // fator de escala
                            .Modifier(FieldValueFactorModifier.Reciprocal) // Reciprocal: - menor preço => maior score
                            .Missing(1) // evita quebrar o cálculo do score na falta do campo 
                        )
                    )   
                    .ScoreMode(FunctionScoreMode.Sum) // Combina score da função com score da query
                    .BoostMode(FunctionBoostMode.Multiply) // Combina resultado final com boost da query
                )
            )
            .Size(take)
        );

        if (!response.IsValid)
        {
            Console.WriteLine($"[OpenSearch] Erro na busca: {response.OriginalException?.Message}");
            return Enumerable.Empty<Game>();
        }

        return response.Documents;
    }

    public async Task<IEnumerable<(string Genre, long Count)>> GetMostPopularGenresAsync()
    {
        var response = await _client.SearchAsync<Game>(s => s
            .Index(_index)
            .Size(0) // não queremos documentos, apenas agregações
            .Aggregations(a => a
                .Terms("popular_genres", t => t
                    .Field(f => f.Genre.Name.Suffix("keyword")) // usar keyword para agrupar por nome exato
                    .Size(10) // limitação - top 10 gêneros
                    .Order(o => o.Descending("_count"))
                )
            )
        );

        var agg = response.Aggregations?.Terms("popular_genres");
        if (agg == null)
        {
            return Enumerable.Empty<(string, long)>();
        }

        // Aqui você pode converter GenreId para string se quiser nomes
        return agg.Buckets.Select(b => (b.Key as string ?? "", b.DocCount ?? 0));
    }
}