using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class TopRatedBooksSpec : Specification<Book>
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
