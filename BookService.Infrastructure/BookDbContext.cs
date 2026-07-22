using BookService.Application.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookService.Infrastructure
{
    public class BookDbContext : DbContext, IBookDbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> opts) : base(opts) { }

        public DbSet<Domain.Entities.Book> Books { get; set; }
        public DbSet<Domain.Entities.Author> Authors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Domain.Entities.Book>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).HasMaxLength(250).IsRequired();
                b.HasOne(x => x.Author).WithMany().HasForeignKey(x => x.AuthorId);
            });
        }
    }
}
