﻿using Microsoft.EntityFrameworkCore;
using WasteZero.Data;
using WasteZero.Models;

namespace WasteZero.Services {
    public class ProductService {
        private readonly WasteZeroDbContext? dbContext;

        public ProductService(WasteZeroDbContext context) {
            dbContext = context;
        }
        public IQueryable<Product>? GetAllObjectsQuerable() {
            IQueryable<Product>? result = dbContext?.Products?.Include("ProductType").OrderBy(x => x.ExpirationDate).AsQueryable();
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
    }
}
