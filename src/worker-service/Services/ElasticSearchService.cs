using Nest;
using worker_service.Models;

namespace worker_service.Services;

public class ElasticSearchService : IElasticSearchService
{
    private string indexName = "logs";
    private readonly IElasticClient _elasticClient;
    public ElasticSearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }
    
    public string InsertLog(Log log)
    {
        if (!_elasticClient.Indices.Exists(indexName).Exists)
        {
            var newIndexName = indexName + System.DateTime.Now.Ticks;
            var indexSettings = new IndexSettings();
            indexSettings.NumberOfReplicas = 1;
            indexSettings.NumberOfShards = 3;

            var response = _elasticClient.Indices.Create(newIndexName, index =>
                index.Map<Log>(m => m.AutoMap())
                    .InitializeUsing(new IndexState() {Settings = indexSettings}).Aliases(a => a.Alias(indexName)));
        }

        IndexResponse indexResponse = _elasticClient.Index<Log>(log, idx => idx.Index(indexName));
        return indexResponse.Id;
    }
}