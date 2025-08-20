using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid UserId { get; set; }
        public required User User { get; set; } // Navigasi ke User
        public required ICollection<TransactionItem> Items { get; set; }
    }
}
