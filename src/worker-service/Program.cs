using worker_service;
using worker_service.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        services.AddHostedService<Worker>();
        services.AddRepositories(hostContext.Configuration);
        services.AddServices();
        services.AddClients();
    })
    .Build();

await host.RunAsync();