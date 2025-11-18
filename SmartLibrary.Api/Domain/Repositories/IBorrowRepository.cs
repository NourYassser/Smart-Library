using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Domain.Repositories
{
    public interface IBorrowRepository
    {
        Task AddAsync(BorrowRecord record, CancellationToken ct = default);
        Task<BorrowRecord?> GetActiveBorrowAsync(Guid bookId, Guid userId, CancellationToken ct = default);
    }
}
