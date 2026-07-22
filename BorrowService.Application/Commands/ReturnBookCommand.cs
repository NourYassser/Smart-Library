using Ardalis.Specification.EntityFrameworkCore;
using BookService.Application.Interface;
using BorrowService.Application.Interface;
using BorrowService.Application.Specs;
using Library.BuildingBlocks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BorrowService.Application.Commands
{
    public record ReturnBookCommand(Guid Barcode, string UserName, string Pin) : IRequest<OperatingResult>;

    public class ReturnBookHandler : IRequestHandler<ReturnBookCommand, OperatingResult>
    {

        private readonly IAuthServiceClient _userClient;
        private readonly IBookServiceClient _bookClient;
        private readonly IBorrowDbContext _context;

        public ReturnBookHandler(
              IAuthServiceClient userClient,
            IBookServiceClient bookClient,
            IBorrowDbContext context)
        {
            _userClient = userClient;
            _bookClient = bookClient;
            _context = context;
        }

        public async Task<OperatingResult> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var user = await _userClient.GetByUsernameAsync(
                request.UserName,
                cancellationToken
            );

            if (user is null) return new OperatingResult()
            {
                IsSuccess = false,
                Message = $"Not finding any results that goes like: {request.UserName}"
            };
            //if (!user.VerifyPin(request.Pin)) return false;

            var book = await _bookClient.GetBookAsync(request.Barcode, cancellationToken);
            if (book is null) return new OperatingResult()
            {
                IsSuccess = false,
                Message = $"No result matchs: {request.Barcode}"
            };
            var record = await _context.Borrow
                .WithSpecification(new ActiveBorrowByUserAndBookSpec(user.Id, book.Id))
                .FirstOrDefaultAsync(cancellationToken);

            if (record is null) return new OperatingResult()
            {
                IsSuccess = false,
                Message = $"No active borrow record found for user {user.Id} and book {book.Id}"
            };
            if (record.ReturnedAt != null) return new OperatingResult()
            {
                IsSuccess = false,
                Message = $"The book with barcode {request.Barcode} has already been returned."
            };
            if (record.UserId != user.Id) return new OperatingResult()
            {
                IsSuccess = false,
                Message = $"The book with barcode {request.Barcode} was not borrowed by user {user.Id}."
            };
            var defaultLoanDays = 14;

            var elapsedDays = (int)Math.Floor((DateTime.UtcNow - record.BorrowedAt).TotalDays);
            var daysLate = Math.Max(0, elapsedDays - defaultLoanDays);
            /*
                        decimal fine = daysLate * book.DailyFine;

                        book.Return(daysLate);
                        record.Return(book.DailyFine + fine);

                        await _bookRepo.UpdateAsync(book, cancellationToken)*/
            ;
            _context.Borrow.Update(record);

            await _context.SaveChangesAsync(cancellationToken);

            return new OperatingResult()
            {
                IsSuccess = true,
                Message = $"Book with barcode {request.Barcode} returned successfully."
            };
        }
    }
}
