using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Api.Domain.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }

        private Author() { }

        public Author([Required] string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
