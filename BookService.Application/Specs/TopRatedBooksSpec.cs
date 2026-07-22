using Ardalis.Specification;

namespace BookService.Application.Specs
{
    public class TopRatedBooksSpec : Specification<Domain.Entities.Book>
    {
        public TopRatedBooksSpec(int take)
        {
            Query.Where(b => b.CopiesAvailable > 0);

            Query.Include(b => b.Author);

            Query.OrderByDescending(b => b.Id);

            Query.Take(take);
        }
    }
}
