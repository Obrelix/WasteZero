﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WasteZero.Data;
using WasteZero.Models;

namespace WasteZero.Services {
    public class ProductService {
        private readonly WasteZeroDbContext? dbContext;

        public ProductService(WasteZeroDbContext context) {
            dbContext = context;
        }
        public IQueryable<Product>? GetAllObjectsQuerable() {
            //dbContext?.ChangeTracker.Clear();
            IQueryable<Product>? result = dbContext?.Products?
            .Include(x=>x.ProductType)
            .Include(x => x.Details)
            .Include(x=>x.ConsumedDetails)
            .AsQueryable();
            return result;
        }
        public List<Product>? GetExpiredObjects() {
            List<ExprirationStatus> expiredObjects = new List<ExprirationStatus>() { ExprirationStatus.almost ,ExprirationStatus.soon, ExprirationStatus.expired };
            List<Product>? result = dbContext?.Products?
            .Include(x => x.ProductType)
            .Include(x => x.Details)
            .ToList();
            if(result!=null && result.Any()) {
                result = result.Where(x => x.Status != null && expiredObjects.Contains((ExprirationStatus)x.Status))
                .OrderBy(x => x.MinDetailExpDate)
                .ThenBy(x => x.Name)
                .ToList();
            }
            return result;
        }
        public IQueryable<ProductDetail>? GetAllDetailsQuerable(Guid parentID) {
            IQueryable<ProductDetail>? result = dbContext?.ProductDetails?.Where(x => x.ProductID.Equals(parentID)).OrderBy(x => x.ExpirationDate).AsQueryable();
            return result;
        }
        public void CancelEdit(Product obj) {
            var entry = dbContext?.Entry(obj);
            if (entry?.State == EntityState.Modified) {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }
        }
        public void DeleteRow(Product obj) {
            if (obj != null) {
                if(obj.Details != null) 
                    foreach (ProductDetail detail in obj.Details) 
                        dbContext?.Remove<ProductDetail>(detail);
                if (obj.ConsumedDetails != null)
                    foreach (ConsumedDetail cd in obj.ConsumedDetails)
                        dbContext?.Remove<ConsumedDetail>(cd);
                dbContext?.Remove<Product>(obj);
                dbContext?.SaveChanges();
            }
        }
        public void UpdateObj(Product obj) {
            if (obj != null) {
                dbContext?.Update<Product>(obj);
                dbContext?.SaveChanges();
            }
        }
        public void Create(Product obj) {
            if (obj != null) {
                dbContext?.Add(obj);
                dbContext?.SaveChanges();
            }
        }
        public void CancelEditDetail(ProductDetail obj) {
            var entry = dbContext?.Entry(obj);
            if (entry?.State == EntityState.Modified) {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }
        }
        public void DeleteRowDetail(ProductDetail obj) {
            if (obj != null) {
                dbContext?.Remove<ProductDetail>(obj);
                dbContext?.SaveChanges();
            }
        }
        public void ConsumeRowDetail(ProductDetail obj) {
            if (obj != null) {
                ConsumedDetail cd = new ConsumedDetail() {
                    Id = Guid.NewGuid(),
                    AddedDate = DateTime.Now,
                    ProductID = obj.ProductID,
                    ExpirationDate = obj.ExpirationDate,
                    Weight = obj.Weight,
                };
                dbContext?.Add<ConsumedDetail>(cd);
                dbContext?.Remove<ProductDetail>(obj);
                dbContext?.SaveChanges();
            }
        }
        public void UpdateObjDetail(ProductDetail obj) {
            if (obj != null) {
                dbContext?.Update<ProductDetail>(obj);
                dbContext?.SaveChanges();
            }
        }
        public void CreateDetail(ProductDetail obj) {
            if (obj != null) {
                dbContext?.Add(obj);
                dbContext?.SaveChanges();
            }
        }
    }
}
