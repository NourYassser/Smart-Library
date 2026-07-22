using Ardalis.Specification;

namespace BookService.Application.Specs
{
    public class AuthorByNameSpec : Specification<Domain.Entities.Author>
    {
        public AuthorByNameSpec(string name)
        {
            var normalized = (name ?? string.Empty).Trim().ToLowerInvariant();
            Query.Where(a => a.Name.ToLower() == normalized);
        }
    }
}
