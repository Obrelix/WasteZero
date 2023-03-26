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
        public IEnumerable<ProductDetail>? Details { get; set; }
        [NotMapped]
        public int? Quantity {
            get {
                if (this.Details == null)
                    return 0;
                else
                    return this.Details?.Count();
            }
        }
        [NotMapped] 
        public float? Weight {
            get {
                if(this.Details == null) 
                    return 0.0f;
                else
                    return this.Details?.Select(x => x.Weight).Sum();
            }
        }
        [NotMapped]
        public ExprirationStatus? Status {
            get {
                if (this.Details == null || !this.Details.Any())
                    return ExprirationStatus.noDetails;
                else {
                    DateTime? minExpDate = this.Details.Where(x=> x.ExpirationDate != null).Select(x=>x.ExpirationDate).Min();
                    if(!minExpDate.HasValue)
                        return ExprirationStatus.noDetails;
                    if (minExpDate <= DateTime.Now.Date)
                        return ExprirationStatus.expired;
                    else if (minExpDate <= DateTime.Now.Date.AddDays(15))
                        return ExprirationStatus.almost;
                    else if (minExpDate <= DateTime.Now.Date.AddDays(30))
                        return ExprirationStatus.soon;
                }
                return ExprirationStatus.ok;
            }
        }
    }
    public enum ExprirationStatus {
        noDetails,
        ok,
        soon,
        almost,
        expired        
    }
}
