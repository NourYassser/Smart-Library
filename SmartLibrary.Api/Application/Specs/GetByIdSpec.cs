using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class GetByIdSpec : Specification<Book>
    {
        public GetByIdSpec(Guid id)
        {
            Query.Where(b => b.Id == id)
            .Include(b => b.Author)
            .AsNoTracking();
        }
    }
}
