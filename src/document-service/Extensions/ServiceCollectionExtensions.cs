using Core.Repositories.Settings;
using document_service.Clients.MessageQueueClient;
using document_service.Helpers.JWT;
using document_service.Repositories;
using document_service.Services;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace document_service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        services.AddSingleton<IMongoSettings>(d=>d.GetRequiredService<IOptions<MongoSettings>>().Value);
            
        services.AddSingleton<IDocumentRepository, DocumentRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
            
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDocumentService, DocumentService>();
        services.AddSingleton<IUserService, UserService>();
        return services;
    }
    
    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        //services.AddSingleton<IUserClient, UserClient>();
        services.AddSingleton<IMessageQueueClient, RabbitMQClient>();
        services.AddHttpClient();
        return services;
    }
    
    public static IServiceCollection AddUtilities(this IServiceCollection services)
    {
        services.AddSingleton<IJwtHelper, JwtHelper>();
        return services;
    }
}