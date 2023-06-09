﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WasteZero.Models
{
    [Table("Products")]
    public class Product {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool? IsGlutenFree { get; set; }
        public string? Name { get; set; }
        [ForeignKey("ProductType")]
        public Guid? ProductTypeID { get; set; }
        public ProductType? ProductType { get; set; }
        public IEnumerable<ProductDetail>? Details { get; set; }
        public IEnumerable<ConsumedDetail>? ConsumedDetails { get; set; }
        [NotMapped]
        public List<ProductDetail>? ExpiredDetails {
            get {
                if (this.Details == null)
                    return new List<ProductDetail>();
                else
                    return this.Details.Where(x=>x.Status!= ExprirationStatus.ok && x.Status != ExprirationStatus.noDetails).ToList();
            }
        }
        [NotMapped]
        public int? ExpiredQuantity {
            get {
                if (this.ExpiredDetails == null)
                    return 0;
                else
                    return this.ExpiredDetails.Count();
            }
        }
        [NotMapped]
        public float ExpiredWeight {
            get {
                if (this.ExpiredDetails == null)
                    return 0.0f;
                else {
                    float? sum = this.ExpiredDetails?.Where(x=>x.Weight.HasValue).Sum(x => x.Weight!.Value);
                    if (sum.HasValue)
                        return (float)Math.Round(sum.Value, 3);
                    else return 0.0f;
                }
            }
        }
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
        public float Weight {
            get {
                if(this.Details == null) 
                    return 0.0f;
                else {
                    float? sum = this.Details?.Where(x => x.Weight.HasValue).Sum(x => x.Weight!.Value);
                    if (sum != null && sum.HasValue)
                        return (float)Math.Round(sum.Value,3);
                    else return 0.0f;
                }
            }
        }
        [NotMapped]
        public ExprirationStatus? Status {
            get {
                if (this.Details == null || !this.Details.Any())
                    return ExprirationStatus.noDetails;
                else {
                    DateTime? minExpDate = this.Details.Where(x=> x.ExpirationDate != null).Min(x=>x.ExpirationDate);
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

        [NotMapped]
        public DateTime MinDetailExpDate { get {
                if (Details!= null && Details.Any()) {
                    DateTime? min = Details.Select(x => x.ExpirationDate).Min();
                    if (min == null || !min.HasValue)
                        return DateTime.MaxValue;
                    else
                        return min.Value;
                }
                else
                    return DateTime.MaxValue;
            } }
    }
    public enum ExprirationStatus {
        noDetails,
        ok,
        soon,
        almost,
        expired        
    }
}
