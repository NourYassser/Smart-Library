namespace SmartLibrary.Api.Domain.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; private set; }
        private Author() { }
        public Author(string name) => Name = name;
    }
}
