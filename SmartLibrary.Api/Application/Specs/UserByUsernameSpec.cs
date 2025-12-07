using Ardalis.Specification;
using SmartLibrary.Api.Domain.Entities;

namespace SmartLibrary.Api.Application.Specs
{
    public class UserByUsernameSpec : Specification<AppUser>
    {
        public UserByUsernameSpec(string username)
        {
            Query.Where(x => x.Username == username);
        }
    }

}
