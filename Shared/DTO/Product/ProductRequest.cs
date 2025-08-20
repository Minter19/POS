using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Product
{
    public class ProductRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; } = "";
        public required Decimal Price { get; set; } = new Decimal(0);
    }
}
