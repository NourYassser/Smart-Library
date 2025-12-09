using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class UserByUsernameSpec : Specification<AppUser>
    {
        public UserByUsernameSpec(string username)
        {
            var n = (username ?? string.Empty).Trim();
            Query.Where(u => u.Username == n);
        }
    }

}
