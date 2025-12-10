using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class ReviewsByBookSpec : Specification<Review>
    {
        public ReviewsByBookSpec(Guid bookId)
        {
            Query.Where(r => r.BookId == bookId);
        }
    }
}
