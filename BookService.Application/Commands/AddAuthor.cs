using BookService.Application.Interface;
using BookService.Domain.Entities;
using Library.BuildingBlocks.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookService.Application.Commands
{
    public record AddAuthorCommand([Required] string Name) : IRequest<OperatingResult>;
    public class AddAuthorHandler : IRequestHandler<AddAuthorCommand, OperatingResult>
    {
        private readonly IBookDbContext _context;

        public AddAuthorHandler(IBookDbContext context) => _context = context;

        public async Task<OperatingResult> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author(request.Name);
            var added = await _context.Authors.AddAsync(author, cancellationToken);
            return new OperatingResult()
            {
                IsSuccess = true,
                Message = "Added author with name: " + added.Entity.Name
            };
        }
    }
}
