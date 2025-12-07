using SmartLibrary.Api.Application.Specs;
using SmartLibrary.Api.Domain.Entities;
using SmartLibrary.Api.Domain.Repositories;

namespace SmartLibrary.Api.FileHelper
{
    public interface IUserContextProvider
    {
        Task<AppUser> GetCurrentUserAsync(CancellationToken cancellationToken);
    }

    public class UserContext : IUserContextProvider
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRepository<AppUser> _repo;

        public UserContext(IHttpContextAccessor context, IRepository<AppUser> repo)
        {
            _httpContext = context;
            _repo = repo;
        }

        public async Task<AppUser> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            var username = _httpContext.HttpContext?.Request.Headers["X-Username"].ToString();
            var pin = _httpContext.HttpContext?.Request.Headers["X-Pin"].ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pin))
                throw new Exception("Missing user identification");

            var user = await _repo.FirstOrDefaultAsync(
                new UserByCredentialsSpec(username, pin), cancellationToken);

            if (user == null)
                throw new Exception("Invalid username or pin");

            return user;
        }
    }

}
