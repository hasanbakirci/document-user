using MassTransit;

namespace worker_service.Consumers;

public class CreateDocumentEventConsumerDefinition : ConsumerDefinition<CreateDocumentEventConsumer>{
    public CreateDocumentEventConsumerDefinition()
    {
        EndpointName = "create-request-queue";
        ConcurrentMessageLimit = 10;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateDocumentEventConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100,200,500,800,1000));
        endpointConfigurator.UseInMemoryOutbox();
    }
}