using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class AuthorByNameSpec : Specification<Author>
    {
        public AuthorByNameSpec(string name)
        {
            var normalized = (name ?? string.Empty).Trim().ToLowerInvariant();
            Query.Where(a => a.Name.ToLower() == normalized);
        }
    }
}
