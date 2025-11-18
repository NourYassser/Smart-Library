using MediatR;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.Application.Commands.BorrowBook
{
    public record BorrowBookCommand(Guid BookId, Guid UserId) : IRequest<Guid>;


    public class BorrowBookHandler : IRequestHandler<BorrowBookCommand, Guid>
    {
        private readonly IBookRepository _bookRepo; private readonly IBorrowRepository _borrowRepo;


        public BorrowBookHandler(IBookRepository bookRepo, IBorrowRepository borrowRepo)
        {
            _bookRepo = bookRepo; _borrowRepo = borrowRepo;
        }


        public async Task<Guid> Handle(BorrowBookCommand request, CancellationToken ct)
        {
            var book = await _bookRepo.GetByIdAsync(request.BookId, ct) ?? throw new Exception("Book not found");
            book.Borrow();
            await _bookRepo.UpdateAsync(book, ct);


            var br = new BorrowRecord(request.BookId, request.UserId);
            await _borrowRepo.AddAsync(br, ct);
            return br.Id;
        }
    }
}
