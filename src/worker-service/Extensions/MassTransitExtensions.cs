using core.Masstransit.Events;
using core.Mongo.MongoContext;
using MassTransit;
using MassTransit.Transports.Fabric;
using MongoDB.Driver;
using worker_service.Consumers;
using worker_service.Repositories;
using worker_service.Services;
using ExchangeType = RabbitMQ.Client.ExchangeType;

namespace worker_service.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitCfg(this IServiceCollection services)
    {
        /**************************************************/
        services.AddMassTransit(mt =>
        {
            mt.AddConsumer<CreateDocumentEventConsumer>(typeof(CreateDocumentEventConsumerDefinition));
            mt.AddConsumer<UpdateDocumentEventConsumer>(typeof(UpdateDocumentEventConsumerDefinition));
            mt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost","/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ReceiveEndpoint("create-request-queue", re =>
                {
                    re.ConfigureConsumeTopology = false;

                    re.ConfigureConsumer<CreateDocumentEventConsumer>(context);
                    re.Bind("request-exchange", e =>
                    {
                        e.RoutingKey = "create.*";
                        e.ExchangeType = ExchangeType.Topic;
                    });
                });
                cfg.ReceiveEndpoint("update-request-queue", re =>
                {
                    re.ConfigureConsumeTopology = false;

                    re.ConfigureConsumer<UpdateDocumentEventConsumer>(context);
                    re.Bind("request-exchange", e =>
                    {
                        e.RoutingKey = "update.*";
                        e.ExchangeType = ExchangeType.Topic;
                    });
                });
            });
            
        });

        return services;
    }
}