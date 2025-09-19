using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class TransactionItem
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public required Transaction Transaction { get; set; } // Navigasi ke Transaction
        public Guid ProductId { get; set; }
        public required Product Product { get; set; } // Navigasi ke Product
        [Range(1, 999999)]
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
