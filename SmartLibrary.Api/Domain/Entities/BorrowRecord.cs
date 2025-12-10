namespace SmartLibrary.Api.Domain.Entities
{
    public class BorrowRecord : BaseEntity
    {
        public Guid BookId { get; private set; }
        public Guid UserId { get; private set; }
        public bool Borrowed { get; set; }
        public DateTime BorrowedAt { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnedAt { get; private set; }
        public decimal FinePaid { get; private set; }
        public int RenewalsCount { get; private set; }
        public Book Book { get; private set; }

        private BorrowRecord() { }

        public BorrowRecord(Guid bookId, Guid userId, DateTime dueDate)
        {
            Id = Guid.NewGuid();
            BookId = bookId;
            UserId = userId;
            Borrowed = true;
            BorrowedAt = DateTime.UtcNow;
            DueDate = dueDate;
            RenewalsCount = 0;
        }
        public BorrowRecord(Guid bookId, Guid userId) : this(bookId, userId, DateTime.UtcNow) { }
        public void Renew(int extensionDays)
        {
            DueDate = DueDate.AddDays(extensionDays);
            RenewalsCount++;
        }
        public void Return(decimal fine)
        {
            ReturnedAt = DateTime.UtcNow;
            FinePaid = fine;
            Borrowed = false;
        }
    }
}
