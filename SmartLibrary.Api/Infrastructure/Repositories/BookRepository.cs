using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _db;
        public BookRepository(LibraryDbContext db) => _db = db;


        public async Task AddAsync(Book book, CancellationToken ct = default)
        {
            await _db.Books.AddAsync(book, ct);
            await _db.SaveChangesAsync(ct);
        }


        public async Task<Book?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id, ct);


        public async Task UpdateAsync(Book book, CancellationToken ct = default)
        {
            _db.Books.Update(book);
            await _db.SaveChangesAsync(ct);
        }


        public async Task<List<Book>> ListAsync(ISpecification<Book> spec, CancellationToken ct = default)
        {
            var evaluator = new Ardalis.Specification.EntityFrameworkCore.SpecificationEvaluator();
            var query = _db.Set<Book>().AsQueryable();
            var finalQuery = evaluator.GetQuery(query, spec);
            return await finalQuery.ToListAsync(ct);
        }
    }
}
