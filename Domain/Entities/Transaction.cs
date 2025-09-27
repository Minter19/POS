using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Transactions", Schema = "Template")]
    public class Transaction
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid UserId { get; set; }
        public required ICollection<TransactionItem> Items { get; set; }
    }
}
