namespace SmartLibrary.Api.Application.DTOs
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string BarCode { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int CopiesAvailable { get; set; }
    }
}
