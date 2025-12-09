using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class BooksByAuthorSpec : Specification<Book>
    {
        public BooksByAuthorSpec(Guid authorId)
        {
            Query
                .Where(b => b.AuthorId == authorId)
                .Include(b => b.Author);
        }
    }
}
