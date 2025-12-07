namespace SmartLibrary.Api.Domain.Entities
{
    public class BorrowRecord : BaseEntity
    {
        public Guid BookId { get; private set; }
        public Guid UserId { get; private set; }
        public bool Borrowed { get; set; }
        public DateTime BorrowedAt { get; private set; }
        public DateTime? ReturnedAt { get; private set; }
        public decimal FinePaid { get; private set; }

        private BorrowRecord() { }

        public BorrowRecord(Guid bookId, Guid userId)
        {
            Id = Guid.NewGuid();
            BookId = bookId;
            UserId = userId;
            Borrowed = true;
            BorrowedAt = DateTime.UtcNow;
        }

        public void Return(decimal fine)
        {
            ReturnedAt = DateTime.UtcNow;
            FinePaid = fine;
            Borrowed = false;
        }
    }
}
