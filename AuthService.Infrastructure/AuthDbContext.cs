using AuthService.Application.Interface;
using AuthService.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure
{
    public class AuthDbContext : DbContext, IAuthDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> opts) : base(opts) { }

        public DbSet<AppUser> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
