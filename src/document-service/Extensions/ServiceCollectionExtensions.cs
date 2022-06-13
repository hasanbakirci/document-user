using core.Mongo.MongoContext;
using core.Settings;
using document_service.Clients.MessageQueueClient;
using document_service.Helpers.JWT;
using document_service.Repositories;
using document_service.Services;
using MassTransit;
using MediatR;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoSettings = core.Mongo.MongoSettings.MongoSettings;

namespace document_service.Extensions;

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
            
        services.AddSingleton<IDocumentRepository, DocumentRepository>();
        services.AddSingleton<IUserRepository, UserRepository>();
            
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(Program));
        services.AddSingleton<IDocumentService, DocumentService>();
        // services.AddSingleton<IDocumentService, DocumentService>(sp => new DocumentService(
        //     sp.GetRequiredService<IDocumentRepository>(),
        //     sp.GetRequiredService<IUserService>(),
        //     sp.GetRequiredService<IPublishEndpoint>()));
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
        
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Bearer Authentication with JWT Token",
                Type = SecuritySchemeType.Http
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        } 
                    },
                    new List<string>()
                }
            });
        });
        
        return services;
    }
}