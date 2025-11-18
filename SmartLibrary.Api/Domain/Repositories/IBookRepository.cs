using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Domain.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Book book, CancellationToken ct = default);
        Task UpdateAsync(Book book, CancellationToken ct = default);
        Task<List<Book>> ListAsync(ISpecification<Book> spec, CancellationToken ct = default);
    }
}
