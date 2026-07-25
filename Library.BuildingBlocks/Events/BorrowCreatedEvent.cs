namespace Library.BuildingBlocks.Events
{
    public record BorrowCreatedEvent(
    Guid BorrowId,
    string BookId,
    Guid UserId,
    DateTime BorrowedAt) : IntegrationEvent
    {
        public override string EventName => "borrow.created";
    };
}
