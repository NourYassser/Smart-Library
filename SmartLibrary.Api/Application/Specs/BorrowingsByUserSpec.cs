using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class BorrowingsByUserSpec : Specification<BorrowRecord>
    {
        public BorrowingsByUserSpec(Guid userId)
        {
            Query
                .Where(b => b.UserId == userId)
                .Include(b => b.Book)
                .OrderByDescending(b => b.BorrowedAt);
        }
    }
}
