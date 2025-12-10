namespace SmartLibrary.Api.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; private set; }
        public Guid AuthorId { get; private set; }
        public Author Author { get; private set; }
        public int CopiesAvailable { get; private set; }
        public decimal DailyFine { get; private set; }
        public string? Barcode { get; private set; }

        private Book() { }


        public Book(string title, Guid authorId, int copies, decimal dailyFine)
        {
            Title = title;
            AuthorId = authorId;
            CopiesAvailable = copies;
            DailyFine = dailyFine;
        }
        public void SetBarcode(string barcode)
        {
            Barcode = string.IsNullOrWhiteSpace(barcode) ? null : barcode.Trim();
        }

        public void Update(string title, string author, int copies)
        {
            Title = title;
            Author.Name = author;
            CopiesAvailable = copies;
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