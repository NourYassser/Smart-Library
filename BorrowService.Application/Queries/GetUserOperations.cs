using Ardalis.Specification.EntityFrameworkCore;
using BookService.Application.Interface;
using BorrowService.Application.DTOs;
using BorrowService.Application.Interface;
using BorrowService.Application.Specs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BorrowService.Application.Queries
{
    public record GetUserOperationsQuery(string Username) : IRequest<UserOperationsDto>;
    public class GetUserOperationsHandler : IRequestHandler<GetUserOperationsQuery, UserOperationsDto>
    {
        private readonly IAuthServiceClient _userClient;
        private readonly IBookServiceClient _bookClient;
        private readonly IBorrowDbContext _context;

        public GetUserOperationsHandler(
            IAuthServiceClient userClient,
            IBookServiceClient bookClient,
            IBorrowDbContext context)
        {
            _userClient = userClient;
            _context = context;
            _bookClient = bookClient;
        }

        public async Task<UserOperationsDto> Handle(GetUserOperationsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userClient.GetByUsernameAsync(
                request.Username,
                cancellationToken);

            if (user == null)
            {
                return new UserOperationsDto(new List<BorrowingDto>(), new List<FineDto>());
            }

            var borrowings = await _context.Borrow
                .WithSpecification(new BorrowingsByUserSpec(user.Id))
                .ToListAsync();

            var borrowDtos = new List<BorrowingDto>();

            foreach (var borrow in borrowings)
            {
                var book = await _bookClient.GetBookAsync(borrow.BookId, cancellationToken);

                borrowDtos.Add(new BorrowingDto(
                    borrow.Id,
                    borrow.BookId,
                    book?.Title ?? "Unknown",
                    borrow.BorrowedAt,
                    borrow.ReturnedAt));
            }

            var fineDtos = borrowings.Select(a => new FineDto(
                a.BookId,
                a.FinePaid,
                a.BorrowedAt,
                a.ReturnedAt
                )
            ).ToList();

            return new UserOperationsDto(borrowDtos, fineDtos);
        }
    }

}
