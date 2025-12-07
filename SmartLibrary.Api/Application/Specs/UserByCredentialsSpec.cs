using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class UserByCredentialsSpec : Specification<AppUser>
    {
        public UserByCredentialsSpec(string username, string pin)
        {
            Query.Where(x => x.Username == username && x.PinCode == pin);
        }
    }
}
