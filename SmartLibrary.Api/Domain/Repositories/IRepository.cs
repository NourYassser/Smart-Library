using Ardalis.Specification;

namespace SmartLibrary.Api.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        //base
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken);
        Task<List<T>> ListAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);

        //app-user
        //Task<AppUser?> GetByIdAsync(Guid id);
    }
}
