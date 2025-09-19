namespace Shared.DTO.Product
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; } = "";
        public required Decimal Price { get; set; } = new Decimal(0);
    }
}
