using Ardalis.Specification.EntityFrameworkCore;
using BookService.Application.Interface;
using BookService.Application.Specs;
using BookService.Domain.Entities;
using Library.BuildingBlocks.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BookService.Application.Commands
{
    public record AddBookCommand(
        string Title,
        string AuthorName,
        int Copies,
        decimal DailyFine
        ) : IRequest<OperatingResult>;


    public class AddBookHandler : IRequestHandler<AddBookCommand, OperatingResult>
    {
        private readonly IBookDbContext _context;

        public AddBookHandler(IBookDbContext context)
        {
            _context = context;
        }

        public async Task<OperatingResult> Handle(AddBookCommand request, CancellationToken ct)
        {
            var authorName = request.AuthorName.Trim();

            var author = await _context.Authors
                                .WithSpecification(new AuthorByNameSpec(authorName))
                                .FirstOrDefaultAsync(ct);

            if (author == null)
            {
                author = new Author(authorName);

                _context.Authors.Add(author);

                await _context.SaveChangesAsync(ct);
            }

            string barcode;

            do
            {
                barcode = RandomNumberGenerator
                    .GetInt32(10_000_000, 100_000_000)
                    .ToString();
            }
            while (await _context.Books.AnyAsync(b => b.Barcode == barcode, ct));

            var book = new Book(
                request.Title,
                author.Id,
                request.Copies,
                request.DailyFine);

            book.SetBarcode(barcode);

            _context.Books.Add(book);

            await _context.SaveChangesAsync(ct);

            return new OperatingResult()
            {
                IsSuccess = true,
                Message = $"Book '{request.Title}' by '{authorName}' added successfully with barcode '{barcode}'."
            };
        }

    }
}
