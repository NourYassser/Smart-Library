using Ardalis.Specification.EntityFrameworkCore;
using BookService.Application.Interface;
using BorrowService.Application.Interface;
using BorrowService.Application.Specs;
using BorrowService.Domain;
using Library.BuildingBlocks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BorrowService.Application.Commands
{
    public record BorrowBookCommand(
     Guid Id,
     string Username,
     string Pin
    )
    : IRequest<OperatingResult>;

    public class BorrowBookHandler
        : IRequestHandler<BorrowBookCommand, OperatingResult>
    {
        private readonly IAuthServiceClient _userClient;
        private readonly IBookServiceClient _bookClient;
        private readonly IBorrowDbContext _context;

        public BorrowBookHandler(
          IAuthServiceClient userClient,
        IBookServiceClient bookClient,
        IBorrowDbContext context)
        {
            _userClient = userClient;
            _bookClient = bookClient;
            _context = context;
        }

        public async Task<OperatingResult> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            var user = await _userClient.GetByUsernameAsync(
            request.Username,
            cancellationToken
        );

            if (user is null)
                throw new Exception("Invalid username or pin");

            /* if (!user.VerifyPin(request.Pin))
                 throw new UnauthorizedAccessException("Invalid username or pin.");*/

            var book = (await _bookClient.GetBookAsync(request.Id, cancellationToken));
            if (book is null)
                throw new InvalidOperationException("Book not found.");

            var existing = await _context.Borrow
                .WithSpecification(new ActiveBorrowByUserAndBookSpec(user.Id, book.Id))
                .FirstOrDefaultAsync(cancellationToken);

            if (existing is not null)
                return new OperatingResult
                {
                    IsSuccess = false,
                    Message = "You have already borrowed this book."
                };

            /*if (book.CopiesAvailable <= 0) throw new InvalidOperationException("No copies available.");

            book.Borrow();

            await _bookRepo.UpdateAsync(book, cancellationToken);*/

            var borrowRecord = new BorrowRecord(book.Id, user.Id);

            var added = await _context.Borrow.AddAsync(borrowRecord, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new OperatingResult
            {
                IsSuccess = true,
                Message = "Book borrowed successfully."
            };
        }
    }

}
