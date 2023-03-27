using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WasteZero.Models {
    [Table("ConsumedDetails")]
    public class ConsumedDetail {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("ProductType")]
        public Guid? ProductID { get; set; }
        public Product? Product { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public float? Weight { get; set; }
    }
}