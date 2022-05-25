using core.Masstransit;
using core.Masstransit.Events;
using MassTransit;
using RabbitMQ.Client;

namespace document_service.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitCfg(this IServiceCollection services)
    {
        services.AddMassTransit(mt =>
        {
            mt.UsingRabbitMq((context,cfg) =>
            {
                cfg.Host("localhost","/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                // cfg.Message<ICreateDocumentEvent>(e => e.SetEntityName("create-request-exchange"));
                // cfg.Publish<ICreateDocumentEvent>(e => e.ExchangeType = ExchangeType.Direct);
                // cfg.Send<ICreateDocumentEvent>(e =>
                // {
                //     e.UseRoutingKeyFormatter(context => "create.service");
                // });
                //
                // cfg.Message<IUpdateDocumentEvent>(e => e.SetEntityName("update-request-exchange"));
                // cfg.Publish<IUpdateDocumentEvent>(e => e.ExchangeType = ExchangeType.Direct);
                // cfg.Send<IUpdateDocumentEvent>(e =>
                // {
                //     e.UseRoutingKeyFormatter(context => "update.service");
                // });
                cfg.Message<IRequestDocumentEvent>(e => e.SetEntityName("request-exchange"));
                cfg.Publish<IRequestDocumentEvent>(e => e.ExchangeType = ExchangeType.Topic);
                cfg.Send<IRequestDocumentEvent>(e =>
                {
                    e.UseRoutingKeyFormatter(context =>
                    {
                        var messageType = context.Message.IsCreate ? "create" : "update";
                        return $"{messageType}.service";
                    });
                });
            });
        });
        return services;
    }
}