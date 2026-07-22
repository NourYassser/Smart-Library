using Ardalis.Specification;
using AuthService.Domain;

namespace AuthService.Application.Specs
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
