using Core.Repositories.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using user_service.Repositories;
using user_service.Services;

namespace user_service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        services.AddSingleton<IMongoSettings>(d=>d.GetRequiredService<IOptions<MongoSettings>>().Value);
            
        services.AddSingleton<IUserRepository, UserRepository>();
            
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        return services;
    }
}