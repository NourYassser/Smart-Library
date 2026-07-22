using Ardalis.Specification.EntityFrameworkCore;
using BookService.Application.Interface;
using BorrowService.Application.Interface;
using BorrowService.Application.Specs;
using Library.BuildingBlocks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BorrowService.Application.Commands
{
    public record ReturnBatchByBarcodesCommand(List<Guid> Barcodes, string UserName, string Pin) : IRequest<OperatingResult>;

    public class ReturnBatchByBarcodesHandler : IRequestHandler<ReturnBatchByBarcodesCommand, OperatingResult>
    {

        private readonly IAuthServiceClient _userClient;
        private readonly IBookServiceClient _bookClient;
        private readonly IBorrowDbContext _context;
        private readonly IConfiguration _configuration;

        public ReturnBatchByBarcodesHandler(
              IAuthServiceClient userClient,
            IBookServiceClient bookClient,
           IBorrowDbContext context,
            IConfiguration configuration)
        {
            _userClient = userClient;
            _bookClient = bookClient;
            _context = context;
            _configuration = configuration;
        }

        public async Task<OperatingResult> Handle(ReturnBatchByBarcodesCommand request, CancellationToken cancellationToken)
        {
            var user = await _userClient.GetByUsernameAsync(request.UserName, cancellationToken);
            //if (user is null || !user.VerifyPin(request.Pin)) return 0;

            var success = 0;
            foreach (var barcode in request.Barcodes.Distinct())
            {
                var book = await _bookClient.GetBookAsync(barcode, cancellationToken);
                if (book is null) continue;

                var record = await _context.Borrow
                    .WithSpecification(new ActiveBorrowByUserAndBookSpec(user.Id, book.Id))
                    .FirstOrDefaultAsync(cancellationToken);

                if (record is null) continue;

                var now = DateTime.UtcNow;
                var daysLate = Math.Max(0, (int)Math.Floor((now - record.DueDate).TotalDays));
                /*var fine = daysLate * book.DailyFine;

                book.Return(daysLate);
                record.Return(fine);

                await _bookRepo.UpdateAsync(book, cancellationToken);*/
                _context.Borrow.Update(record);

                _context.SaveChangesAsync(cancellationToken);

                success++;
            }

            return new OperatingResult
            {
                IsSuccess = true,
                Message = $"Successfully returned {success} out of {request.Barcodes.Count} books."
            };
        }
    }
}
