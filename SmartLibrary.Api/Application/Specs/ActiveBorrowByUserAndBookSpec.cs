using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class ActiveBorrowByUserAndBookSpec : Specification<BorrowRecord>
    {
        public ActiveBorrowByUserAndBookSpec(Guid userId, Guid bookId)
        {
            Query
                .Where(r => r.UserId == userId && r.BookId == bookId && r.Borrowed == true);
        }
    }
}
