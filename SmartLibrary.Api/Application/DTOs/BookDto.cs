using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Api.Application.DTOs
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string BarCode { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int CopiesAvailable { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }

    public class ReviewDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        [MaxLength(5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
