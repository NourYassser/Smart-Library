using Library.BuildingBlocks.Events;

namespace Library.BuildingBlocks.Messaging
{
    public interface IEventBus
    {
        Task PublishAsync(
        IntegrationEvent @event,
        CancellationToken cancellationToken = default);
    }
}
