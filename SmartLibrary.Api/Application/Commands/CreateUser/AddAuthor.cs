using MediatR;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Api.Application.Commands.CreateUser
{
    public record AddAuthorCommand([Required] string Name) : IRequest<Guid>;
    public class AddAuthorHandler : IRequestHandler<AddAuthorCommand, Guid>
    {
        private readonly IRepository<Author> _repository;

        public AddAuthorHandler(IRepository<Author> repository) => _repository = repository;

        public async Task<Guid> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author(request.Name);
            var added = await _repository.AddAsync(author, cancellationToken);
            return added.Id;
        }
    }
}
