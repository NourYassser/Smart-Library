using BorrowService.Domain;
using Microsoft.EntityFrameworkCore;

namespace BorrowService.Application.Interface
{
    public interface IBorrowDbContext
    {
        DbSet<BorrowRecord> Borrow { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
