using MassTransit;

namespace worker_service.Consumers;

public class UpdateDocumentEventConsumerDefinition : ConsumerDefinition<UpdateDocumentEventConsumer>
{
    public UpdateDocumentEventConsumerDefinition()
    {
        EndpointName = "update-request-queue";
        ConcurrentMessageLimit = 10;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UpdateDocumentEventConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100,200,500,800,1000));
        endpointConfigurator.UseInMemoryOutbox();
    }
}