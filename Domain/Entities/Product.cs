using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Common.Constants;
namespace Domain.Entities
{
    //[Table("Products")] //Custom init nama tabel
    [Table("Products", Schema = "Template")] //custom init nama tabel dan schema per entity
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Description("The name of product")]
        [MaxLength(MaximumLengthFor.LongText)]
        public required string Name { get; set; }

        [MaxLength(MaximumLengthFor.MultilineText)]
        public string? Description { get; set; }
        
        public required decimal Price { get; set; }
        
        [Range(0, 999999)]
        public int Stock { get; set; }
    }
}
