using Microsoft.EntityFrameworkCore;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Infrastructure
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> opts) : base(opts) { }


        public DbSet<AppUser> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).HasMaxLength(250).IsRequired();
                b.HasOne(x => x.Author).WithMany().HasForeignKey(x => x.AuthorId);
            });
        }
    }
}
