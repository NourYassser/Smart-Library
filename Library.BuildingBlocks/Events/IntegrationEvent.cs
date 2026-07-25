namespace Library.BuildingBlocks.Events
{
    public abstract record IntegrationEvent
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;

        public abstract string EventName { get; }
    }
}
