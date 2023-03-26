using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WasteZero.Models {
    [Table("ProductDetails")]
    public class ProductDetail {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("ProductType")]
        public Guid? ProductID { get; set; }
        public Product? Product { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? AddedDate { get; set; }
        public float? Weight { get; set; }

        [NotMapped]
        public ExprirationStatus? Status {
            get {
                if (!this.ExpirationDate.HasValue)
                    return ExprirationStatus.noDetails;
                else if (this.ExpirationDate <= DateTime.Now.Date)
                    return ExprirationStatus.expired;
                else if (this.ExpirationDate <= DateTime.Now.Date.AddDays(15))
                    return ExprirationStatus.almost;
                else if (this.ExpirationDate <= DateTime.Now.Date.AddDays(30))
                    return ExprirationStatus.soon;
                return ExprirationStatus.ok;
            }
        }
    }
}
