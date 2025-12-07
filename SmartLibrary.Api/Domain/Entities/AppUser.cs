namespace SmartLibrary.Api.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public string Username { get; set; } = default!;
        public string PinCode { get; set; } = default!;
    }

}
