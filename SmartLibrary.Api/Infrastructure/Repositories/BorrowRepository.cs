using Microsoft.EntityFrameworkCore;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Infrastructure.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibraryDbContext _context;
        public BorrowRepository(LibraryDbContext context) => _context = context;

        public async Task AddAsync(Domain.Entities.BorrowRecord borrowRecord, CancellationToken ct = default)
        {
            await _context.BorrowRecords.AddAsync(borrowRecord, ct);
            await _context.SaveChangesAsync(ct);
        }
        public async Task<Domain.Entities.BorrowRecord?> GetActiveBorrowAsync(Guid bookId, Guid userId, CancellationToken ct = default)
        {
            return await _context.BorrowRecords
                .FirstOrDefaultAsync(br => br.BookId == bookId && br.UserId == userId && br.ReturnedAt == null, ct);
        }
    }
}
