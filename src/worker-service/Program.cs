using core.Masstransit.Events;
using MassTransit;
using worker_service;
using worker_service.Consumers;
using worker_service.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        //services.AddHostedService<Worker>();
        services.AddRepositories(hostContext.Configuration);
        services.AddServices();
        services.AddClients();
        /**************************************************/
        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateDocumentEventConsumer>();
            x.AddRequestClient<ICreateDocumentEvent>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", e =>
                {
                    e.Username("guest");
                    e.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        /**************************************************/
        services.AddMassTransit<ISecondBus>(x =>
        {
            x.AddConsumer<UpdateDocumentEventConsumer>();
            x.AddRequestClient<IUpdateDocumentEvent>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", e =>
                {
                    e.Username("guest");
                    e.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    })
    .Build();

await host.RunAsync();

public interface ISecondBus:IBus{}