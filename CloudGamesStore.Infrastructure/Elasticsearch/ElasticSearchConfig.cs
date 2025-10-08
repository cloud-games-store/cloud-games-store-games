using CloudGamesStore.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenSearch.Client;

namespace CloudGamesStore.Infrastructure.Elasticsearch;

public static class ElasticSearchConfig
{
    public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["ElasticSearch:Url"] ?? "http://localhost:9200";
        var defaultIndex = configuration["ElasticSearch:Index"] ?? "CloudGames-Games";

        var settings = new ConnectionSettings(new Uri(url))
            .DefaultIndex(defaultIndex)
            .PrettyJson()
            .DisableDirectStreaming()
            .DefaultMappingFor<Game>(m => m.IdProperty(p => p.Id));

        var client = new OpenSearchClient(settings);

        if (!client.Indices.Exists(defaultIndex).Exists)
        {
            client.Indices.Create(defaultIndex, i => i.Map<Game>(m => m.AutoMap()));
        }

        services.AddSingleton<IOpenSearchClient>(client);
    }
}
