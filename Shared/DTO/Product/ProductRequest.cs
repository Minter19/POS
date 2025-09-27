using Shared.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Product
{
    public class ProductRequest
    {
        [Required]
        [MaxLength(MaximumLengthFor.LongText)]
        public required string Name { get; set; }

        [MaxLength(MaximumLengthFor.MultilineText)]
        public string? Description { get; set; } = "";
        public required Decimal Price { get; set; } = new Decimal(0);
        public required int Stock { get; set; } = 0;
    }
}
