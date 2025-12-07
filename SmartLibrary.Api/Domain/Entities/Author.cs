namespace SmartLibrary.Api.Domain.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        private Author() { }
        public Author(string name) => Name = name;
    }
}
