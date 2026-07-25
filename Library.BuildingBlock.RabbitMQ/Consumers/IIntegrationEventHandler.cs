namespace Library.BuildingBlocks.RabbitMQ.Consumers
{
    public interface IIntegrationEventHandler<in TEvent>
    {
        Task HandleAsync(
            TEvent @event,
            CancellationToken cancellationToken);
    }
}
