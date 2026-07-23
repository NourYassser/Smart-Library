using Ardalis.Specification;

namespace BookService.Application.Specs
{
    public class GetByIdSpec : Specification<Domain.Entities.Book>
    {
        public GetByIdSpec(string id)
        {
            Query.Where(b => b.Barcode == id)
            .Include(b => b.Author)
            .AsNoTracking();
        }
    }
}
