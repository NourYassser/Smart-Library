using Ardalis.Specification;

namespace BookService.Application.Specs
{
    public class GetByIdSpec : Specification<Domain.Entities.Book>
    {
        public GetByIdSpec(Guid id)
        {
            Query.Where(b => b.Id == id)
            .Include(b => b.Author)
            .AsNoTracking();
        }
    }
}
