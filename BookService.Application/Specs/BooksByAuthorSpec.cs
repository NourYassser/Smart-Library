using Ardalis.Specification;

namespace BookService.Application.Specs
{
    public class BooksByAuthorSpec : Specification<Domain.Entities.Book>
    {
        public BooksByAuthorSpec(Guid authorId)
        {
            Query
                .Where(b => b.AuthorId == authorId)
                .Include(b => b.Author);
        }
    }
}
