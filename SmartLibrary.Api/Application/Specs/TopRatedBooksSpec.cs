using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class TopRatedBooksSpec : Specification<Book>
    {
        public TopRatedBooksSpec(int take)
        {
            Query.Where(b => b.CopiesAvailable > 0)
            .Include(b => b.Author)
            .OrderByDescending(b => b.Id)
            .Take(take);
        }
    }
}
