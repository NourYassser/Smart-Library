using Ardalis.Specification;
using AuthService.Domain;

namespace AuthService.Application.Specs
{
    public class UserByCredentialsSpec : Specification<AppUser>
    {
        public UserByCredentialsSpec(string username, string pin)
        {
            Query.Where(x => x.Username == username && x.PinCode == pin);
        }
    }
}
