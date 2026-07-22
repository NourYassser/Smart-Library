using AuthService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Interface
{
    public interface IAuthDbContext
    {
        DbSet<AppUser> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
