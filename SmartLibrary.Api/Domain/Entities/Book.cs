namespace SmartLibrary.Api.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; private set; }
        public Guid AuthorId { get; private set; }
        public Author Author { get; private set; }
        public int CopiesAvailable { get; private set; }
        public decimal DailyFine { get; private set; }


        private Book() { }


        public Book(string title, Guid authorId, int copies, decimal dailyFine)
        {
            Title = title;
            AuthorId = authorId;
            CopiesAvailable = copies;
            DailyFine = dailyFine;
        }


        public void Borrow()
        {
            if (CopiesAvailable <= 0) throw new InvalidOperationException("No copies available");
            CopiesAvailable--;
        }


        public void Return(int daysLate)
        {
            CopiesAvailable++;
        }
    }
}