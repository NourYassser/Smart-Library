using Ardalis.Specification;
using BorrowService.Domain;

namespace BorrowService.Application.Specs
{
    public class BorrowingsByUserSpec : Specification<BorrowRecord>
    {
        public BorrowingsByUserSpec(Guid userId)
        {
            Query
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BorrowedAt);
        }
    }
}
