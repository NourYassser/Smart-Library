using BorrowService.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Interface
{
    public interface IBorrowDbContext
    {
        DbSet<BorrowRecord> Borrow { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
