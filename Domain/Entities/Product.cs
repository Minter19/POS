using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        [Range(0, 999999)]
        public int Stock { get; set; }
    }
}
