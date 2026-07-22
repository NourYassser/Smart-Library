using BookService.Application.Interface;
using BorrowService.Domain;
using Microsoft.EntityFrameworkCore;

namespace BorrowService.Infrastructure
{
    public class BorrowDbContext : DbContext, IBorrowDbContext
    {
        public BorrowDbContext(DbContextOptions<BorrowDbContext> opts) : base(opts) { }

        public DbSet<BorrowRecord> Borrow { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
