using core.Mongo.MongoContext;
using core.Mongo.MongoSettings;
using core.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Nest;
using worker_service.Clients.MessageQueueClient;
using worker_service.Repositories;
using worker_service.Services;

namespace worker_service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        // BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
        // BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));
        //
        // services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        // services.AddSingleton<IMongoSettings>(d=>d.GetRequiredService<IOptions<MongoSettings>>().Value);
        MongoSettings settings = configuration.GetSection("MongoSettings").Get<MongoSettings>();
        services.AddSingleton(settings);

        var mongoClient = new MongoClient(settings.Server);
        var mongoContext = new MongoContext(mongoClient, settings.Database);

        services.AddSingleton<IMongoContext, MongoContext>(m => mongoContext);
        
            
        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        ElasticSearchSettings settings = configuration.GetSection("ElasticSearchSettings").Get<ElasticSearchSettings>();
        
        var connection = new ConnectionSettings(new Uri(settings.Server));
        var client = new ElasticClient(connection);
        services.AddSingleton<IElasticSearchService, ElasticSearchService>(x => new ElasticSearchService(client));
        //services.AddHostedService<Worker>();
        return services;
    }
    
    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        //services.AddSingleton<IMessageQueueClient, RabbitMQClient>();
        return services;
    }
}