using BookService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Interface
{
    public interface IBookDbContext
    {
        DbSet<Book> Books { get; }

        DbSet<Author> Authors { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
