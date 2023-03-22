using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WasteZero.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool? IsGlutenFree { get; set; }
        public string? Name { get; set; }
        [ForeignKey("ProductType")]
        public Guid? ProductTypeID { get; set; }
        public ProductType? ProductType { get; set; }
        public float? Quantity { get; set; }
        public float? Weight { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
