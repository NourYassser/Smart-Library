using Ardalis.Specification.EntityFrameworkCore;
using AuthService.Application.DTOs;
using AuthService.Application.Interface;
using AuthService.Application.Specs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Queries
{
    public record GetUserByUserNameQuery(string UserName) : IRequest<UserDto?>;

    public class GetUserByUserNameQueryHandler : IRequestHandler<GetUserByUserNameQuery, UserDto?>
    {
        private readonly IAuthDbContext _context;
        public GetUserByUserNameQueryHandler(IAuthDbContext context)
        {
            _context = context;
        }
        public async Task<UserDto?> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .WithSpecification(new UserByUsernameSpec(request.UserName))
                .FirstOrDefaultAsync(cancellationToken);
            return new UserDto()
            {
                Id = user?.Id ?? Guid.Empty,
                UserName = user?.Username ?? string.Empty
            };
        }
    }




}
