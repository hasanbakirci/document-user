using core.Masstransit.Events;
using MassTransit;
using RabbitMQ.Client;
using worker_service;
using worker_service.Consumers;
using worker_service.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        //services.AddHostedService<Worker>();
        services.AddRepositories(hostContext.Configuration);
        services.AddServices(hostContext.Configuration);
        services.AddClients();
        services.AddMassTransitCfg();
    })
    .Build();

await host.RunAsync();

public interface ISecondBus:IBus{}