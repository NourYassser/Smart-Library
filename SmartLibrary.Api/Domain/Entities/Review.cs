namespace SmartLibrary.Api.Domain.Entities
{
    public class Review : BaseEntity
    {
        public Guid BookId { get; private set; }
        public Guid UserId { get; private set; }
        public int Rating { get; private set; } // 1..5
        public string? Text { get; private set; }
        public DateTime CreatedOn { get; private set; }

        private Review() { }

        public Review(Guid bookId, Guid userId, int rating, string? text)
        {
            BookId = bookId;
            UserId = userId;
            Rating = Math.Clamp(rating, 1, 5);
            Text = text;
            CreatedOn = DateTime.UtcNow;
        }
    }
}
